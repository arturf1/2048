using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// a sound controller is needed
[RequireComponent(typeof(AudioSource))]

// for each cell we create an instantiation which is able to move or combine with others
public class Tile : MonoBehaviour {

	public Manager gameManager; 

	public Text textFab;
	// simple sound when a cell moves
	public AudioClip FX;
	// value which is written on each cell
	public int tileValue;
	// same value? Do a combinaison...
	public bool combined;
	
	Vector2 movePosition;
	bool combine;
	Tile cTile;
	bool grow;
	public int x;
	public int y;

	void Start () {
		// position of our cell and creation
		movePosition = transform.localPosition;
		//textFab = (GameObject)Instantiate (textFab,transform.position,Quaternion.Euler(0,0,0));
		Change (tileValue);
	}

	void FixedUpdate () {

		//textFab.GetComponent<GUIText>().transform.position = Camera.main.WorldToViewportPoint (transform.position);
		if(transform.localPosition != new Vector3(movePosition.x,movePosition.y,0f)) {
			gameManager.done = false;
			// we move our cell slowly
			transform.localPosition = Vector3.MoveTowards(transform.localPosition,movePosition, 35 * Time.fixedDeltaTime * Time.timeScale);
		} else {
			gameManager.done = true;
			// do a combinaison and increase valur on cell
			if(combine) {
				Change(tileValue * 2); // new value
				combine = false;
				grow = true;
				// destroy our cell after it has combined with other
				//Destroy(cTile.textFab);
				//Destroy(cTile.gameObject);
				cTile.delete();
				// play a sound when cell combines with another
				GetComponent<AudioSource>().PlayOneShot(FX, 1.0f);
				gameManager.done = true;
			}
		}
		// create a scale FX when it spawns
		if(transform.localScale.x != 150 && !grow)
			transform.localScale = Vector3.MoveTowards(transform.localScale,new Vector3(150f,150f,1f), 500 * Time.fixedDeltaTime * Time.timeScale);
		if(grow) { // create a scale FX when cell combines with another
			gameManager.done = false;
			transform.localScale = Vector3.MoveTowards(transform.localScale,new Vector3(187.5f,187.5f,1f), 500 * Time.fixedDeltaTime * Time.timeScale);
			if(transform.localScale == new Vector3(187.5f,187.5f,1f))
                grow = false;
		} else
            gameManager.done = true;
	}

	void Change (int newValue) {

		tileValue = newValue;
		// after combination we change tile's colour
		GetComponent<SpriteRenderer>().color = Manager.tileColors [Mathf.RoundToInt(Mathf.Log (tileValue, 2) - 1)];
		//textFab.GetComponent<GUIText>().text = tileValue.ToString();
		textFab.text = tileValue.ToString();
		// colour value which is written on our cell
		//textFab.GetComponent<GUIText>().color = new Color (0.17f, 0.17f, 0.27f);
	}

	public bool Move (int x, int y) {

		movePosition = gameManager.gridToWorld (x, y);

		if(transform.localPosition != (Vector3)movePosition) {

			if(gameManager.grid [x, y] != null) {
				combine = true;
				combined = true;
				cTile = gameManager.grid[x,y];
				gameManager.grid[x,y] = null;
			} 
			// move the cell
			gameManager.grid[this.x,this.y] = null;
			gameManager.grid[x,y] = GetComponent<Tile>();
			this.x = x; 
			this.y = y;
			return true;

		} else
            return false;
	}

	public void delete() {
		Destroy (gameObject);
	}
}