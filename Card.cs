using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gacha_Game
{
    enum Rarity
    {
        Error,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    class Card
    {

        private Image cardImage;
        private string cardName;
        private int cardId;
        private Rarity rarity;
        private int count;
        private int labelName;

        public Card(Image cardImage, string cardName, int cardId, Rarity rarity)
        {
            this.cardImage = cardImage;
            this.cardName = cardName;
            this.cardId = cardId;
            this.rarity = rarity;
            this.count = 1;
            labelName = 0;
        }

        public Image CardImage
        {
            get
            {
                return cardImage;
            }
        }

        public string CardName
        {
            get
            {
                return cardName;
            }
        }

        public int CardId
        {
            get
            {
                return cardId;
            }
        }

        public Rarity Rarity
        {
            get
            {
                return rarity;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public int LabelName
        {
            get
            {
                return labelName;
            }

            set
            {
                labelName = value;
            }
        }

        public void IncrementCount()
        {
            count++;
        }

        
    }
}
