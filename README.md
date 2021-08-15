<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#Agent Design">Agent Design</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>

## About The Project

https://user-images.githubusercontent.com/8249946/129461000-7ee95073-adc7-4552-99dd-881ba665d0d6.mp4

If you never played it before, 2048 is a puzzle game. Tiles with values of 2 or 4 spawn randomly on a 4 by 4 grid. On each turn, you choose to shift all of the tiles up, down, left or right. Tiles with the same value combine into one tile with sum of the original tiles. Tiles "4" and "4" will combine to create "8". Tiles "8" and "8" will combine to create "16". The goal of the game is to create a tile with value of 2048. You can try to play it yourself at: https://play2048.co/

## Agent Design 

### Observation 
The game grid is represented as an array of 16 numbers. Each element is 0 if the cell is empty or log base-2 of the tile value times 1/11. The core idea is to evenly space out all of the tile values between 0 and 1.

I did consider using one hot encoding. That would lead to a 16*12 input space, and most of the observation would be zero most of the time. Therefore, I did not try it. But it could be something to experiment with.

### Reward Function 
The are two reward functions. 

The first function, used in most of the experiments, includes:
* penalty for actions which do not increase the score
* reward for every increase in total score
* reward for every new highest tile found (discounted, see graph below)
* reward for getting to 2048

![Discounted reward for new highest tile found](Recordings/2048_reward_functions.PNG)

The second function, also called Simple Reward, includes:
* reward for every new highest tile found

### Training

The agent is a 4 layer FFN trained with PPO. In general, I set a small beta (0.001) for entropy, and a large buffer (12,800) as the agent will need to do a lot of exploration. Also, gamma is large (0.995) for the discount to make sure the agent is looking many moves ahead. For specific hyperparameter settings please see YAML files in the config folder. 

Actions which do not change the state of the game are masked to prevent the agent from getting stuck. 

## Agent for Simplified 2048

The trickier part is the dynamics of the game. The tiles spawn in random locations and with random values. This makes it harder for the agent to learn a good combination of moves. Also, the game gets exponential harder. It is a lot harder to get tile with value of 512 than a tile of 256. Lastly, certain moves do not change the state of the game. This can lead to an agent getting stuck.

For the initial proof-of-concept I simplified the game by only spawning tiles with value 2 and in deterministic locations. To handle the increasing difficulty, I designed a reward function to provide a reward for each time tiles are combined, each time a new high value tile is created, and for winning the game. There is also a penalty for each move which does not result in tiles combining. To prevent the agent from getting stuck, I mask each action which did not result in a change to the game gird in the previous move.

This agent is called deterLocDeterValwNormMask. Below is the training curve and a video of full game. 

![Training Curve for Simplified 2048 Agent](Recordings/2048_simplified_deterLocDeterValwNormMask_training.PNG)

[![Simplified 2048 Agent Full Game](https://img.youtube.com/vi/eQf2c7eh8LM/0.jpg)](https://youtu.be/eQf2c7eh8LM)

## Agent for 2048

### Complex Rewards 

Agent training curve for simplified 2048 (grey). 
Fine tuning that agent on original 2048 (orange). 
The original version is much harder. 

![Simplified versus Original 2048 Training Curves](Recordings/2048_deterLocDeterValwNormMask-vs-rndLocRndValwNormMask_training.PNG)

rndLocRndValwNormMaskColdStart\Play2048

Agent trained from scratch and a fine-tuned agent converge onto the same strategy. They both move highest value tiles to one side.
Grey: training on simplified 2048  
Orange: fine tuning on original 2048 
Blue: training agent on original 2048 
![Cold Start versus Fine Tune](Recordings/2048_coldstart_vs_finetune.PNG)

rndLocRndValwNormMask\Play2048
rndLocRndValwNormMask_2\Play2048
rndLocRndValwNormMask_3\Play2048

Training curves for fine tuning an agent to play 2048.  
Orange: First round  33M steps
Red: Second round 50M steps
Blue: Third round 12M steps
![Agent Training](Recordings/2048_rndLocRndValwNormMask_1_2_and_3_training.PNG)

After 215,000 games of 2048, this agent can win 4% (+/-3%) of rounds. 
See winning game first row third column in video. 
[![Simplified 2048 Agent Full Game](https://img.youtube.com/vi/3NAvX7lpD5Q/0.jpg)](https://youtu.be/3NAvX7lpD5Q)

### Simple Rewards 

## Getting Started

To get started, first install Unity Game Engine from  https://unity.com/. This project also requires the ML Agents extension. Installation instructions can be found on the extension's Github, link below. 
* [Unity ML Agents](https://github.com/Unity-Technologies/ml-agents)

Once the Engine and ML Agents extension are installed, clone the project repository and import the project using Unity Hub. 

This project also uses the following assets, which are included in the project files. 
* [2048 Project in Unity Asset Store](https://assetstore.unity.com/packages/templates/packs/2048-23088)
* [Gridbox Prototype Materials](https://assetstore.unity.com/packages/2d/textures-materials/gridbox-prototype-materials-129127)

### Prerequisites

This is an example of how to list things you need to use the software and how to install them.
* npm
  ```sh
  npm install npm@latest -g
  ```

### Installation

1. Get a free API Key at [https://example.com](https://example.com)
2. Clone the repo
   ```sh
   git clone https://github.com/your_username_/Project-Name.git
   ```
3. Install NPM packages
   ```sh
   npm install
   ```
4. Enter your API in `config.js`
   ```JS
   const API_KEY = 'ENTER YOUR API';
   ```
   
## License

MIT License

## Contact

[@arturf124](https://twitter.com/arturf124) | [LinkedIn](https://www.linkedin.com/in/filipowicza/)

## Resources

### Unity ML Agents

* [Unity Agent Design](https://github.com/Unity-Technologies/ml-agents/blob/release_2_verified_docs/docs/Learning-Environment-Design-Agents.md#masking-discrete-actions)

* [Unity ML Agent Classes](https://docs.unity3d.com/Packages/com.unity.ml-agents@1.0/api/Unity.MLAgents.html)

* [Training Configuration File](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Training-Configuration-File.md#common-trainer-configurations)

### RL

* [Part 1: Key Concepts in RL](https://spinningup.openai.com/en/latest/spinningup/rl_intro.html#reward-and-return)

* [Part 3: Intro to Policy Optimization](https://spinningup.openai.com/en/latest/spinningup/rl_intro3.html#deriving-the-simplest-policy-gradient)

* [Understanding PPO Plots in TensorBoard](https://medium.com/aureliantactics/understanding-ppo-plots-in-tensorboard-cbc3199b9ba2)

### 2048

* [2048](https://play2048.co/)

* [Is every game of 2048 winnable? If not, what are the odds of any game being winnable, given perfect play?](https://www.quora.com/Is-every-game-of-2048-winnable-If-not-what-are-the-odds-of-any-game-being-winnable-given-perfect-play)

* [Is the game 2048 always solveable?](https://math.stackexchange.com/questions/720726/is-the-game-2048-always-solveable)

* [2048 Game Strategy - How to Always Win at 2048](https://www.gameskinny.com/lnagr/2048-game-strategy-how-to-always-win-at-2048)

