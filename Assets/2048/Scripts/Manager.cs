using UnityEngine;
using System.Collections;
using System.Linq;

public class Manager : MonoBehaviour {
	// colors of our cell
	// here you can define anothers colors in order to create your own game
	public static Color[] tileColors = new Color[12] {
		new Color(0.733f, 0.894f, 0.855f),
		new Color(0.729f, 0.878f, 0.784f),
		new Color(0.749f, 0.794f, 0.475f),
		new Color(0.761f, 0.684f, 0.388f),
		new Color(0.765f, 0.586f, 0.373f),
		new Color(0.715f, 0.499f, 0.331f),
		new Color(0.729f, 0.412f, 0.027f),
		new Color(0.729f, 0.302f, 0.381f),
		new Color(0.729f, 0.224f, 0.314f),
		new Color(0.729f, 0.150f, 0.247f),
		new Color(0.729f, 0.100f, 0.181f),
		new Color(0.235f, 0.025f, 0.196f)};
	// default value for the high score
	private int highscore = 0;

	public static readonly float[] gridY = new float[4] {0.95f,2.65f,4.35f,6.05f};
	public static readonly int[] validDir = new int[4] {0, 1, 2, 3};

	// these values define our board size - you can do a new game using a 5x5 or 6x6 size
	public Tile[,] grid = new Tile[4,4];

	// our GO handles
	public GameObject tileFab;

	public bool randomLocation = true; 
	public bool randomValue = true;

	public bool done;
	// restart values when the game starts or restarts
	public bool spawnWaiting;
	public bool go = true;
	public int score = 0;
	public bool winner;
	public bool gameOver;

	// clear our grid
	void Start () {

		//Cursor.visible = false; // hide cursor
		// we read in our player's preferences file the high score which was saved
		highscore = PlayerPrefs.GetInt("High Score");
		System.Array.Clear (grid, 0, grid.Length);
		done = false;

		// a little tip to call the first and second cell when a new game starts
		Spawn ();
		Spawn ();
		/////////////////////////////////////////////////
	}

	public void Restart() {
		done = false;
		spawnWaiting = false;
		go = true;
		score = 0;
		winner = false;
		gameOver = false;

		// delete all existing tiles in grid
		for (int i = 0; i < grid.GetLength(0); i++)
            for (int j = 0; j < grid.GetLength(1); j++)
                if(grid[i,j] != null) {
					grid[i,j].delete();
				}
		// check for any remaining tiles
		Component[] tiles = GetComponentsInChildren(typeof(Tile));
        if (tiles != null)
        {
            foreach (Tile t in tiles)
                t.delete();
        }

		System.Array.Clear (grid, 0, grid.Length);
		done = false;

		// a little tip to call the first and second cell when a new game starts
		Spawn ();
		Spawn ();
	}

	// line 71 of this coroutine function isn't included in our game. 
	// it's a little tip to simulate a randomly movement - not an IA (see line 146)
	IEnumerator TryToSolveMyProblem() {
		go = false;
	//	time between each movement - 0.33 second
		yield return new WaitForSeconds(0.33f);
    //  Move(Random.Range (0,3));
		go = true;
	}

	void FixedUpdate () {
        // simple escape control which works for application and mobile devices
        if(Input.GetKey(KeyCode.Escape))
            Application.Quit();
        // moment until spawn is done
        if (done && spawnWaiting)
			Spawn ();
		/*#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        // keys to play - arrows on keyboard
        if (done & !gameOver && go == true) {
			if(Input.GetKey(KeyCode.UpArrow))
                Move(0);
			if(Input.GetKey(KeyCode.DownArrow))
                Move(1);
			if(Input.GetKey(KeyCode.LeftArrow))
                Move(2);
			if(Input.GetKey(KeyCode.RightArrow))
                Move(3);
		}
		#endif*/
        //////////////////////////////////////////////////////////
        // boolean winner true - if you have reached a cell 2048, you are the winner
        if(winner) {
			// save the high score
			if(score > highscore) {
				highscore = score;
				PlayerPrefs.SetInt("High Score", highscore);
			} 
			// launch a new game calling the same level
			if (Input.GetKeyDown("space"))
                Application.LoadLevel(0);
		}
		//////////////////////////////////////////////////////////
		// boolean gameOver true
		if(gameOver) {
			// launch a new game calling the same level
			if (Input.GetKeyDown("space"))
                Application.LoadLevel(0);
		}
	}

    public void Move(int dir) {

		if (winner)
            return; // game is out - boolean winner is true (see line 81)
		if (gameOver)
            return; // game is out - boolean game over is true (see line 96)
		if (!done)
            return; // time between each movement is not yet reached
		if (!validDir.Contains(dir))
			return; // direction needs to be valid

		StartCoroutine(TryToSolveMyProblem());
		// game continues
		done = false;
		Vector2 vector = Vector2.zero;
		bool moved = false;
		// define x and y values in our array
		int[] xArray = {0,1,2,3};
		int[] yArray = {0,1,2,3};
		// reverse our array when change direction by one key
		switch(dir) {
			case 0: vector = Vector2.up;
                System.Array.Reverse(yArray);
                break;
			case 1: vector = -Vector2.up;
                break;
			case 2: vector = -Vector2.right;
                break;
			case 3: vector = Vector2.right;
                System.Array.Reverse(xArray);
                break;
		}

		foreach(int x in xArray) {
			foreach(int y in yArray) {
				if(grid[x,y] != null) {
					grid[x,y].combined = false;
					Vector2 cell;
					Vector2 next = new Vector2(x, y);
					do {
						cell = next;
						next = new Vector2(cell.x + vector.x, cell.y + vector.y);
					} while (isInArea(next) && grid[Mathf.RoundToInt(next.x),Mathf.RoundToInt(next.y)] == null);

					int nx = Mathf.RoundToInt(next.x); int ny = Mathf.RoundToInt(next.y);
					int cx = Mathf.RoundToInt(cell.x); int cy = Mathf.RoundToInt(cell.y);
					// if this cell isn't occuped yet - we move it
					if(isInArea(next) && !grid[nx,ny].combined && grid[nx,ny].tileValue == grid[x,y].tileValue) {
						score += grid[x,y].tileValue * 2; // we increase the value of the new cell

						grid[x,y].Move(nx,ny); //combined
						moved = true;
						if((grid[nx,ny].tileValue * 2) == 2048) {
							// 2048 is reached - end of the game - you win
							winner = true;
						}
					} else {
						if(grid[x,y].Move(cx,cy))
							moved = true; //move
					}
				}
			}
		}
		if(moved) { // while one cell can move, game continues
			spawnWaiting = true;
		} else {
			moved = false;
			for(int x = 0; x <= 3; x++) {
				for(int y = 0; y <= 3; y++) {
					if(grid[x,y] == null) {
						moved = true;
					} else {
						for(int z = 0; z <= 3; z++) {
							Vector2 Vtor = Vector2.zero;
							switch(z) { // keys
							case 0:
                                    Vtor = Vector2.up;
                                    break;
							case 1:
                                    Vtor = -Vector2.up;
                                    break;
							case 2:
                                    Vtor = Vector2.right;
                                    break;
							case 3:
                                    Vtor = -Vector2.right;
                                    break;
							}
							if(isInArea(Vtor + new Vector2(x,y)) && 
                                grid[x + Mathf.RoundToInt(Vtor.x), y + Mathf.RoundToInt(Vtor.y)] != null &&
                                grid[x,y].tileValue == grid[x + Mathf.RoundToInt(Vtor.x), y + Mathf.RoundToInt(Vtor.y)].tileValue)
								moved = true;
						}
					}
				}
			}
			// if we cannot move one cell, game is over
			if(!moved)
				gameOver = true;
		}
		done = true;
	}

	bool isInArea(Vector2 vec) {
		return 0 <= vec.x && vec.x <= 3 && 0 <= vec.y && vec.y <= 3;
	}

	void Spawn() {

		spawnWaiting = false;
		bool oc = true;
		int xx = 0;
		int yy = 0;

		if (randomLocation){
			// spawn a tile in our array random(x)/random(y)
			while (oc) {
				xx = Random.Range(0,4);
				yy = Random.Range(0,4);
				// do it if this cell isn't occuped
				if(grid[xx,yy] == null) oc = false;
			}
		}
		else {
			// spawn a tile in row order
			for (xx = 0; xx < grid.GetLength(0); xx++)
            	for (yy = 0; yy < grid.GetLength(1); yy++)
                	if(grid[xx,yy] == null) {
						goto Exit;
					}
		}

		Exit:
		int startValue = 0;
		if (randomValue){
			// define value to spawn - 4 or sometimes 2
			startValue = 4; // 12% to be a 4
			if(Random.value < 0.88f)
				startValue = 2; // 88% to be a 2
		}
		else {
			startValue = 2;
		}

		GameObject tempTile = (GameObject)Instantiate (tileFab);
		tempTile.transform.parent = gameObject.transform;
		tempTile.transform.localPosition = gridToWorld (xx, yy);
		tempTile.transform.rotation = Quaternion.Euler (0, 0, 0);
		tempTile.GetComponent<Tile>().x = xx;
		tempTile.GetComponent<Tile>().y = yy;
		tempTile.GetComponent<Tile>().gameManager = GetComponent<Manager>();
		grid [xx, yy] = tempTile.GetComponent<Tile>();
		grid [xx, yy].tileValue = startValue;
	}

	public Vector3 gridToWorld(int x, int y) {
		return new Vector3(1.7f * x  + 0.95f, gridY [y], 0);
	}
	
	public Vector2 worldToGrid(float x, float y) {
		for (int i = 0; i <= 3; i++) {
			if(gridY[i] == y) y = i;
		}
		return new Vector2((x - 0.95f)/1.7f, y);
	}
}