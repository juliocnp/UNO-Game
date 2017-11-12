﻿using MauMau.Classes.Background.Cartas;
using MauMau.Classes.Background.Enum;
using MauMau.Classes.Background.Estruturas;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MauMau.Classes.Background
{
    class Bot : Player
    {
        private Canvas ambiente;
        private Monte mnt;
        private Coletor clt;
        private Enginee eng;
        /// <summary>
        /// Lista de todas as cartas do baralho
        /// </summary>
        public Bot(Profile prf) : base(prf)
        {
            this.SetHand(new Lista<Carta>());
            SetProfile(prf);
        }
        public Bot(Lista<Carta> hand, Profile infos, Enginee eng, PlayerPosition position) : base(hand, infos, position)
        {
            this.eng = eng;
            this.ambiente = this.eng.Enviroment;
            this.mnt = this.eng.Monte;
            this.clt = this.eng.Descarte;
            this.SetHand(new Lista<Carta>());
            SetProfile(infos);
        }
        /// <summary>
        /// Simula jogadas de um player
        /// </summary>
        /// <param name="cardMonte">
        /// Recebe a carta do topo do coletor e o Monte</param> 
        public void Jogar()
        {
            Carta ctMenor = null;
            // pega carta do top do coletor como referencia
            Carta cdTop = this.clt.GetTopCard();
            Lista<Carta> listaaux = new Lista<Carta>();
            if(this.hand.Count == 1)
            {
                base.TimeToUNO();
            }
            //prioridade Normal(menor numero) > especial
            foreach (Carta cardMao in GetHand())
            {
                if (cardMao.Compatible(cdTop))
                {
                    listaaux.Add(cardMao);
                }
            }
            if (listaaux.Count > 1)
            {
                foreach (Carta card in listaaux)
                {
                    if (card is Normal)
                    {
                        ctMenor = card;
                        goto Continuar;
                    }
                }
                foreach (Carta card in listaaux)
                {
                    if (card is Especial)
                    {
                        ctMenor = card;
                        goto Continuar;
                    }
                }
                foreach (Carta card in listaaux)
                {
                    if (card is Coringa)
                    {
                        ctMenor = card;
                        goto Continuar;
                    }
                }
            }
            else
            {
                ctMenor = listaaux[0];
            }
            Continuar:
            if (ctMenor != null)
            {
                AnimationHandToColetor(ctMenor);
            }
            else
            {
                Carta added = AnimationMontToHand();
                if (added.Compatible(cdTop))
                {
                    AnimationHandToColetor(ctMenor);
                }
            }
            this.eng.EndTurn();
        }
        private void AnimationHandToColetor(Carta card)
        {
            DoubleAnimation moveAnimY = new DoubleAnimation(Canvas.GetTop(card.ElementUI), Canvas.GetTop(this.eng.Element_colapse), new Duration(TimeSpan.FromMilliseconds(100)));
            DoubleAnimation moveAnimX = new DoubleAnimation(Canvas.GetLeft(card.ElementUI), Canvas.GetLeft(this.eng.Element_colapse), new Duration(TimeSpan.FromMilliseconds(100)));
            card.ElementUI.BeginAnimation(Canvas.TopProperty, moveAnimY);
            card.ElementUI.BeginAnimation(Canvas.LeftProperty, moveAnimX);
        }
        private Carta AnimationMontToHand()
        {
            Carta getcard = this.eng.RemoveFromMonte();
            Rectangle cardUI = getcard.ElementUI;
            Canvas.SetLeft(cardUI as UIElement, Canvas.GetLeft(this.eng.MonteUI));
            Canvas.SetTop(cardUI as UIElement, Canvas.GetTop(this.eng.MonteUI));

            this.eng.Enviroment.Children.Add(cardUI);
            Carta aux = this.hand[this.hand.Count - 1];
            DoubleAnimation moveAnimY;
            DoubleAnimation moveAnimX;

            if (base.position == PlayerPosition.Right || base.position == PlayerPosition.Left)
            {
                moveAnimY = new DoubleAnimation(Canvas.GetTop(cardUI), Canvas.GetTop(aux.ElementUI) + 30, new Duration(TimeSpan.FromMilliseconds(100)));
                moveAnimX = new DoubleAnimation(Canvas.GetLeft(cardUI), Canvas.GetLeft(aux.ElementUI), new Duration(TimeSpan.FromMilliseconds(100)));
            }
            else
            {
                moveAnimY = new DoubleAnimation(Canvas.GetTop(cardUI), Canvas.GetTop(aux.ElementUI), new Duration(TimeSpan.FromMilliseconds(100)));
                moveAnimX = new DoubleAnimation(Canvas.GetLeft(cardUI), Canvas.GetLeft(aux.ElementUI) + 40, new Duration(TimeSpan.FromMilliseconds(100)));
            }

            moveAnimX.FillBehavior = FillBehavior.Stop;
            moveAnimY.FillBehavior = FillBehavior.Stop;
            getcard.ElementUI.BeginAnimation(Canvas.TopProperty, moveAnimY);
            getcard.ElementUI.BeginAnimation(Canvas.LeftProperty, moveAnimX);

            Canvas.SetLeft(getcard.ElementUI, Canvas.GetLeft(aux.ElementUI) + 40);
            Canvas.SetTop(getcard.ElementUI, Canvas.GetTop(aux.ElementUI));

            this.hand.Add(getcard);
            return getcard;
        }
    }
}
