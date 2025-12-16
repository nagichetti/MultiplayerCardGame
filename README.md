#Multiplayer Card Game

## 🎯 Overview

This project is a **turn-based 1v1 multiplayer card game** developed in **Unity**, created as part of a technical assignment.
Two players connect online, play multiple cards per turn, and resolve card abilities through a structured **reveal and scoring system**.

The implementation focuses on:

* Networking
* Event-driven gameplay
* Modular card and ability systems
* Clear separation of gameplay, networking, and UI

---

## 🕹️ Game Flow

### Match Rules

* Total turns: **6**
* Deck size: **12 cards**
* Starting hand: **3 cards**
* At the start of each turn: **draw 1 card**
* Turn timer: **30 seconds**
* Player may end the turn early using **End Turn**
* If the timer expires, the turn ends automatically

---

## 🃏 Card Selection & Folding

### Card Selection

* Cards can be selected and unselected freely during the turn
* Selection is only a local preview
* Cards are not locked until the turn ends

### Folding

* Cards are **folded** when the player presses **End Turn**
* Folded cards:

  * Appear face-down on the board
  * Cannot be changed
  * Are used during the reveal phase
* Opponent sees **only the number of folded cards**, not their identity

---

## 🔄 Reveal Phase & Initiative

### Reveal Phase

* Starts only after **both players end their turn**

### Initiative

* Determined **once per turn** at the start of the reveal phase
* Player with the **higher score reveals first**
* If scores are tied, the starting player is chosen randomly
* Initiative does **not change** during the reveal phase

---

## 🔁 Reveal Sequence

Cards are revealed:

* In the order they were played
* In an **alternating sequence**:

  1. Initiative player reveals card #1 → resolve → update score
  2. Opponent reveals card #1 → resolve → update score
  3. Continue alternating until all cards are revealed

After every reveal:

* The score is updated immediately
* The next reveal starts only after the score update is complete

---

## 📊 Cost & Scoring System

* Turn 1 → 1 available cost
* Turn 2 → 2 available cost
* …
* Turn 6 → 6 available cost

Players may play multiple cards per turn as long as:

```
Total card cost ≤ available cost
```

* Each card’s **power adds to the score of the player who played it**
* Some abilities modify scores or affect opponent cards
* The winner is the player with the **highest score after 6 turns**

---

## 🧠 Card System

Cards are defined using data-driven structures and include:

* `id` – Unique identifier
* `name` – Card name
* `cost` – Cost required to play
* `power` – Base power
* `ability.type` – Ability type
* `ability.value` – Integer value used by the ability

### Implemented Abilities

* **GainPoints** – Adds points to the player’s score
* **StealPoints** – Transfers points from opponent to player
* **DoublePower** – Increases card power
* **DrawExtraCard** – Draws additional cards
* **DiscardOppRandomCard** – Discards random card(s) from opponent
* **DestroyOppPlayedCards** – Cancels opponent’s played cards before resolution

---

## 🌐 Networking

* Built using **Unity Netcode for GameObjects**
* Supports **1v1 multiplayer**
* Game state is synchronized between players
* Communication is **event-driven**
* All network messages are sent as **JSON strings**
* Each message contains an `action` field (no raw RPC primitives)

---

## 📡 Event System

Core internal events used:

* `GameStart`
* `TurnStart`
* `PlayerEndedTurn`
* `AllPlayersReady`
* `RevealCard`
* `ScoreUpdated`
* `TurnEnd`
* `GameEnd`

---

## ⚠️ Current Limitations

* ❌ **Game restart is not supported**
* ❌ **Player reconnection is not supported**

Once a match ends or a player disconnects, the game must be restarted manually.

---

## 📦 Deliverables Implemented

* Unity project in a public GitHub repository
* Full 6-turn gameplay logic
* Reveal sequence with correct initiative handling
* Score updates after every reveal
* Modular and event-driven architecture

---

## ✅ Evaluation Focus

* Correct multiplayer gameplay
* Proper reveal and initiative logic
* Event-driven architecture
* Clean, readable, and modular code
* Clear separation of gameplay, networking, and UI systems
