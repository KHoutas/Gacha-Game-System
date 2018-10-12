# Gacha-Game-System
Practiced design and creation of a gacha game system.

## What is a Gacha Game?
A Gacha Game (notably found on mobile devices) is a game that players collect items (whether it be cards or characters) using a random summoning system. This system is named a gacha system, and I am working to recreate a neat little gacha game using cards I edit up. **NONE OF THE ARTWORK IS MINE, I JUST UTILIZED IT FOR RECREATIONAL PURPOSES**

## How it works
A database holds most of the items in the most simplest way possible. Notably, a database with four tables. Tables are of cards, accounts, account information (like currency, level, and experience), and a fourth associative table which links cards to accounts. The gacha system simply allows me to randomize some values, and look for cards based on such a value. (for instance, you randomize a number from 1 - 100, and only getting a 98 - 100 allows you to get a Legendary card. You randomize a 99, so the program will filter just legendary cards, randomize, and pick one for you)

**This is just a bit of practice with logic and database usage. I'd like to learn this to perhaps apply this to the creation of a game in the near future to also add to my portfolio**
