﻿using MauMau.Classes.Background.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MauMau.Classes.Background
{
    class Player
    {
        protected List<Carta> hand;
        protected Profile infos;
        protected PlayerPosition position;

        public Profile Infos { get { return this.infos; } set { this.infos = value; } }
        public List<Carta> Hand { get { return this.hand; } }
        public PlayerPosition Position { get { return this.position; } }

        public Player(Profile infos)
        {
            this.hand = new List<Carta>();
            this.infos = infos;
        }

        public Player(Profile infos, PlayerPosition position)
        {
            this.hand = new List<Carta>();
            this.infos = infos;
            this.position = position;
        }

        public Player(List<Carta> hand, Profile infos)
        {
            this.hand = hand;
            this.infos = infos;
        }

        public Player(List<Carta> hand, Profile infos, PlayerPosition position)
        {
            this.hand = hand;
            this.infos = infos;
            this.position = position;
        }
        /// <summary>
        /// Adiciona uma carta a mão do jogador
        /// </summary>
        /// <param name="card"></param>
        public void AddCardToHand(Carta card)
        {
            this.hand.Add(SetCardAngle(card));
        }
        /// <summary>
        /// Retira uma carta da mão do jogador
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Carta PlayCard(Carta card)
        {
            return Remover(card);
        }
        /// <summary>
        /// Define o angulo da carta com base no jogador
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        protected Carta SetCardAngle(Carta card)
        {
            switch (this.position)
            {
                case PlayerPosition.Left:
                    RotateTransform rotate = new RotateTransform(-90);
                    card.ElementUI.RenderTransform = rotate;
                    return card;
                case PlayerPosition.Right:
                    rotate = new RotateTransform(90);
                    card.ElementUI.RenderTransform = rotate;
                    return card;
                case PlayerPosition.Top:
                    rotate = new RotateTransform(180);
                    card.ElementUI.RenderTransform = rotate;
                    return card;
                default: return card; ;
            }
        }

        /// <summary>
        /// Remove uma carta do baralho
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        private Carta Remover(Carta card)
        {
            for (int i = 0; i < CountHand(); i++)
            {
                if (this.hand[i] == card) hand.RemoveAt(i);
                return card;
            }
            return null;
        }
        /// <summary>
        /// Retorna o número de cartas na mão do jogador
        /// </summary>
        /// <returns></returns>
        public int CountHand()
        {
            return this.hand.Count;
        }
        /// <summary>
        /// Retorna se o jogador pode ou não pedir UNO
        /// </summary>
        /// <returns></returns>
        public bool TimeToUNO()
        {
            if (CountHand() == 1) return true;
            return false;
        }
        public List<Carta> GetHand()
        {
            return this.hand;
        }
        public void SetHand(List<Carta> hd)
        {
            this.hand = hd;
        }
        public Profile GetProfile()
        {
            return this.infos;
        }
        public void SetProfile(Profile prf)
        {
            this.infos = prf;
        }
    }
}