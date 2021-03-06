﻿using System;
using MauMau.Classes.Background.Cartas;
using MauMau.Classes.Background.Enum;
using MauMau.Classes.Background.Util;
using MauMau.Classes.Background.Estruturas;

namespace MauMau.Classes.Background
{
    public class Evento
    {
        /// <summary>
        /// Referência para o motor do jogo
        /// </summary>
        private Enginee eng;
        /// <summary>
        /// Objeto para animações
        /// </summary>
        private Animation anim;
        /// <summary>
        /// Variável auxiliar para a contagem de repetições das animações
        /// </summary>
        private int cardsBuyed;
        /// <summary>
        /// Variável auxiliar para quantidade de repetições que uma animação executará
        /// </summary>
        private int numberofrepetions;
        /// <summary>
        /// Jogador para o qual a animação se destinará
        /// </summary>
        private Player player_destiny;

        public Evento(Enginee e)
        {
            this.eng = e;
            this.anim = new Animation(eng, 900);
            this.anim.MoveAnimX.Completed += MoveAnimX_Completed;
        }

        /// <summary>
        /// Envia duas cartas para a mão do jogador
        /// </summary>
        private void Comprar()
        {
            cardsBuyed++;
            Carta auxcard = this.eng.Monte.RemoveTopCard();
            this.anim.RotateToHand(auxcard, this.player_destiny);

            this.anim.MontToHand(auxcard.ElementUI, this.player_destiny);
            this.player_destiny.AddCardToHand(auxcard);
        }
        /// <summary>
        /// Carrega as informações sobre o usuário que receberá a carta
        /// </summary>
        private void LoadPlayerInfos()
        {
            this.player_destiny = this.eng.Roda.GetNextPlayerInOrder();
            //this.player_destiny = this.eng.Roda.GetPlayerByPosition(PlayerPosition.Bottom);
        }
        /// <summary>
        /// Método auxílio que definirá quantas cartas serão compradas, chamando o método real de compra de cartas
        /// </summary>
        /// <param name="quant"></param>
        private void Comprar(int quant)
        {
            this.numberofrepetions = quant;
            this.LoadPlayerInfos();
            this.Comprar();

            Log.AddEventoComprarCartas(this.player_destiny, numberofrepetions);
        }
        /// <summary>
        /// Evento para quanto a animação terminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveAnimX_Completed(object sender, EventArgs e)
        {
            if (this.cardsBuyed < this.numberofrepetions)
            {
                this.Comprar();
            }
            else
            {
                this.numberofrepetions = this.cardsBuyed = 0;
            }
        }
        /// <summary>
        /// Bloqueia o próximo jogador na lista
        /// </summary>
        private void Bloquear()
        {
            Turno turno = this.eng.Roda;
            Lista<Player> jogadores = turno.Jogadores;
            switch (turno.GetSentido())
            {
                case SentidoJogada.Horario:

                    if (jogadores.GetIndexOf(turno.GetCurrentPlayer()) + 2 >= jogadores.Count)
                    {
                        turno.SetCurrentPlayer(jogadores[0]);
                        Log.AddEventoBloquearJogador(turno.GetCurrentPlayer());
                    }
                    else
                    {
                        turno.SetCurrentPlayer(jogadores[jogadores.GetIndexOf(turno.GetCurrentPlayer()) + 2]);
                        Log.AddEventoBloquearJogador(turno.GetCurrentPlayer());
                    }
                    break;

                case SentidoJogada.AntiHorario:

                    if (jogadores.GetIndexOf(turno.GetCurrentPlayer()) - 2 < 0)
                    {
                        turno.SetCurrentPlayer(jogadores[jogadores.Count - 1]);
                        Log.AddEventoBloquearJogador(turno.GetCurrentPlayer());
                    }
                    else
                    {
                        turno.SetCurrentPlayer(jogadores[jogadores.GetIndexOf(turno.GetCurrentPlayer()) - 1]);
                         Log.AddEventoBloquearJogador(turno.GetCurrentPlayer());
                    }
                    break;
            }
        }

        /// <summary>
        /// Inverte a ordem sequencial de jogada
        /// </summary>
        private void Inverter()
        {
            Turno turno = this.eng.Roda;
            Lista<Player> jogadores = turno.Jogadores;

            switch (turno.GetSentido())
            {
                case SentidoJogada.Horario:
                    turno.SetSentido(SentidoJogada.AntiHorario);

                    Log.AddEventoInverterJogo();
                    break;
                case SentidoJogada.AntiHorario:
                    turno.SetSentido(SentidoJogada.Horario);

                    Log.AddEventoInverterJogo();
                    break;
            }
        }
        /// <summary>
        /// Verifica se uma carta possui efeito
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool HasEfect(Carta card)
        {
            if (card is Especial || card is Curinga) return true;
            else return false;
        }
        /// <summary>
        /// Verifica se a carta é curinga
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool IsJoker(Carta card)
        {
            if (card is Curinga) return true;
            return false;
        }
        /// <summary>
        /// Faz o evento conforme carta jogada.
        /// Retorna false caso a carta jogada for do tipo Curinga
        /// </summary>
        /// <param name="cardJogada"></param>
        public void EventAtivado(Carta cardJogada)
        {
            if (cardJogada is Especial)
            {
                Especial aux = (Especial)cardJogada;

                switch (aux.Efeito)
                {
                    case Efeito.Comprar2:
                        this.Comprar(2);
                        break;
                    case Efeito.Bloquear:
                        this.Bloquear();
                        break;
                    case Efeito.Inverter:
                        this.Inverter();
                        break;
                }
            }
            else if (cardJogada is Curinga) //coringa troca cor
            {
                Curinga aux = (Curinga)cardJogada;
                if (this.eng.GetCurrentPlayer().Position == PlayerPosition.Bottom)
                {
                    this.anim.ShowPaletColors();
                }
                switch (aux.Efeito)
                {
                    case Efeito.MudarCor:
                        break;
                    case Efeito.MudarCorEComprar4:
                        Comprar(4);
                        break;
                }
            }
        }
    }
}

