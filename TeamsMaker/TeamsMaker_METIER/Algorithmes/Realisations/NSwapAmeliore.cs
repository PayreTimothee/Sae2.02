using System.Diagnostics;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;
using TeamsMaker_METIER.Algorithmes.Outils;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class NSwapAmeliore : Algorithme
    {
        ///<author> LAMBERT Hugo</author>
        /// <summary>
        /// Algorithme N-Swap Amélioré
        /// </summary>
        /// <param name="jeuTest"> Jeu de test utilisé </param>
        /// <returns> Repartition contenant les équipes de 4 personnages modifié </returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Extreme_en_premier algoStart = new Extreme_en_premier();
            Repartition repInitial = algoStart.Repartir(jeuTest);

            List<Double> ScoreEquipeTrie = new List<Double>();

            //On trie les équipes par leur score
            foreach (Equipe equipe in repInitial.Equipes)
            {
                if (equipe.Membres.Length == 4)
                {
                    ScoreEquipeTrie.Add(equipe.Score(Probleme.SIMPLE));
                }
            }

            Array.Sort(ScoreEquipeTrie.ToArray());

            Repartition repSwap = repInitial;
            bool meilleur = true;

            while (meilleur )
            {
                meilleur = false;
                bool ameliorationTrouvee = false;

                //On parcoure les équipes qui ont les moyennes les plus basses
                for (int i = 0; i < repInitial.Equipes.Length; i++)
                {
                    //On parcoure les équipes qui ont les moyennes les plus haute
                    for (int j = repInitial.Equipes.Length -1; j > 0 && ameliorationTrouvee != true; j--)
                    {
                        Equipe repEquipeA = repInitial.Equipes[i];
                        Equipe repEquipeB = repInitial.Equipes[j];
                        int a = 0;
                        while (a < repEquipeA.Membres.Length && ameliorationTrouvee != true )
                        {
                            Personnage personnageA = repEquipeA.Membres[a];
                            int b = 0;
                            while (b < repEquipeB.Membres.Length && ameliorationTrouvee != true )
                            {
                                Personnage personnageB = repEquipeB.Membres[b];
                                Equipe equipe1 = new Equipe();
                                Equipe equipe2 = new Equipe();
                                equipe1.AjouterMembre(personnageB);
                                equipe2.AjouterMembre(personnageA);

                                Personnage Personnage1 = null;
                                Personnage Personnage2 = null;

                                //On vérifie que les personnages les plus forts et les plus faibles existent pour éviter les NullReferenceException
                                if (Personnage1 != null && Personnage2 != null)
                                {
                                    equipe1.AjouterMembre(Personnage1);
                                    equipe2.AjouterMembre(Personnage2);

                                    //Ajouter le personnages si ce n'est pas le personnageA ou le personnageB ou les personnages les plus forts et les plus faibles de chaque équipe
                                    foreach (Personnage membreA in repEquipeA.Membres)
                                    {
                                        if (membreA != personnageA && membreA != Personnage1 && membreA != Personnage2)
                                        {
                                            equipe1.AjouterMembre(membreA);
                                        }
                                    }

                                    //Ajouter le personnages si ce n'est pas le personnageA ou le personnageB ou les personnages les plus forts et les plus faibles de chaque équipe
                                    foreach (Personnage membreB in repEquipeB.Membres)
                                    {
                                        if (membreB != personnageB && membreB != Personnage1 && membreB != Personnage2)
                                        {
                                            equipe2.AjouterMembre(membreB);
                                        }
                                    }

                                    //On vérifie que les deux équipes sont valides pour le problème SIMPLE avant de calculer le score
                                    if (equipe1.EstValide(Probleme.SIMPLE) && equipe2.EstValide(Probleme.SIMPLE))
                                    {
                                        double scoreNouvellesEquipes = equipe1.Score(Probleme.SIMPLE) + equipe2.Score(Probleme.SIMPLE);
                                        double scoreAvant = repEquipeA.Score(Probleme.SIMPLE) + repEquipeB.Score(Probleme.SIMPLE);
                                        double differenceScore = scoreNouvellesEquipes - scoreAvant;
                                        if (differenceScore < 0)
                                        {
                                            Repartition nouvelleRep = new Repartition(jeuTest);
                                            nouvelleRep.AjouterEquipe(equipe1);
                                            nouvelleRep.AjouterEquipe(equipe2);
                                            for (int k = 0; k < repSwap.Equipes.Length; k++)
                                            {
                                                if (repSwap.Equipes[k] != repEquipeA && repSwap.Equipes[k] != repEquipeB)
                                                {
                                                    nouvelleRep.AjouterEquipe(repSwap.Equipes[k]);
                                                }
                                            }
                                            repSwap = nouvelleRep;
                                            meilleur = true;
                                            ameliorationTrouvee = true;
                                        }
                                    }
                                    // incrémente
                                    b++;
                                }
                                //incrémente
                                a++;

                            }
                        }
                    }
                }

            }

            //On crée la répartition finale en ne gardant que les équipes dont le nombre de personnage est égale à 4
            Repartition repFinale = new Repartition(jeuTest);
            foreach (Equipe equipe in repSwap.Equipes)
            {
                if (equipe.Membres.Length == 4)
                {
                    repFinale.AjouterEquipe(equipe);
                }
            }
            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            return repFinale;
        }
        
    }
}