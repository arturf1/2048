<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>

## About The Project

https://user-images.githubusercontent.com/8249946/129461000-7ee95073-adc7-4552-99dd-881ba665d0d6.mp4

![Product Name Screen Shot](Recordings/2048_reward_functions.PNG)

Last week I decided to build an AI agent for the game 2048. If you never played it before, 2048 is a puzzle game. Tiles with values of 2 or 4 spawn randomly on a 4 by 4 grid. On each turn, you choose to shift all of the tiles up, down, left or right. Tiles with the same value combine into one tile with sum of the original tiles. Tiles "4" and "4" will combine to create "8". Tiles "8" and "8" will combine to create "16". The goal of the game is to create a tile with value of 2048. You can try to play it yourself at: https://play2048.co/

At first, I thought the challenging part will be designing an output for the agent. I thought that each tile can be moved individually. It turns out that there are only four actions. That makes the output of the agent very simple.

The trickier part is the dynamics of the game. The tiles spawn in random locations and with random values. This makes it harder for the agent to learn a good combination of moves. Also, the game gets exponential harder. It is a lot harder to get tile with value of 512 than a tile of 256. Lastly, certain moves do not change the state of the game. This can lead to an agent getting stuck.

For the initial proof-of-concept I simplified the game by only spawning tiles with value 2 and in deterministic locations. To handle the increasing difficulty, I designed a reward function to provide a reward for each time tiles are combined, each time a new high value tile is created, and for winning the game. There is also a penalty for each move which does not result in tiles combining. To prevent the agent from getting stuck, I mask each action which did not result in a change to the game gird in the previous move.

### Built With

This section should list any major frameworks that you built your project using. Leave any add-ons/plugins for the acknowledgements section. Here are a few examples.
* [Bootstrap](https://getbootstrap.com)
* [JQuery](https://jquery.com)
* [Laravel](https://laravel.com)

## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

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

## Agent for Simplified 2048

## Agent for 2048

### Original Trial 

### Simple Rewards 

Use this space to show useful examples of how a project can be used. Additional screenshots, code examples and demos work well in this space. You may also link to more resources.

_For more examples, please refer to the [Documentation](https://example.com)_

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

