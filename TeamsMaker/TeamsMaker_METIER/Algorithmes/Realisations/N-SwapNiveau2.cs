using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    internal class N_SwapNiveau2 : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            Heuristique1_niveau2 algoStart = new Heuristique1_niveau2();
            Repartition repInitial = algoStart.Repartir(jeuTest);
            Repartition repSwap = repInitial;
            bool meilleur = true;
            while (meilleur)
            {
                meilleur = false;
                for (int i = 0; i < repInitial.Equipes.Length; i++)
                {
                    for (int j = i + 1; j < repInitial.Equipes.Length; j++)
                    {
                        Equipe repEquipeA = repInitial.Equipes[i];
                        Equipe repEquipeB = repInitial.Equipes[j];
                        foreach (Personnage personnageA in repEquipeA.Membres)
                        {
                            foreach (Personnage personnageB in repEquipeB.Membres)
                            {
                                Equipe equipe1 = new Equipe();
                                Equipe equipe2 = new Equipe();
                                equipe1.AjouterMembre(personnageB);
                                equipe2.AjouterMembre(personnageA);

                                //On vérifie si le score de l'équipe 1 est inférieur à 50 et celui de l'équipe 2 est supérieur à 50 pour le problème ROLEPRINCIPAL
                                if (equipe1.Score(Probleme.ROLEPRINCIPAL) < 50 && equipe2.Score(Probleme.ROLEPRINCIPAL) > 50)
                                {
                                    //Initalisation des personnages les plus forts et les plus faibles de chaque équipe
                                    Personnage? lePlusFortEquipeA = null;
                                    Personnage? lePlusNulEquipeA = null;

                                    Personnage? lePlusFortEquipeB = null;
                                    Personnage? lePlusNulEquipeB = null;

                                    //On parcourt les membres des équipes pour trouver les personnages les plus forts et les plus faibles
                                    foreach (Personnage perso in repEquipeA.Membres)
                                    {
                                        if (perso.LvlPrincipal > lePlusFortEquipeA.LvlPrincipal)
                                        {
                                            lePlusFortEquipeA = perso;
                                        }

                                        if (perso.LvlPrincipal < lePlusNulEquipeA.LvlPrincipal)
                                        {
                                            lePlusNulEquipeA = perso;
                                        }
                                    }

                                    foreach (Personnage perso in repEquipeB.Membres)
                                    {
                                        if (perso.LvlPrincipal > lePlusFortEquipeB.LvlPrincipal)
                                        {
                                            lePlusFortEquipeB = perso;
                                        }

                                        if (perso.LvlPrincipal < lePlusNulEquipeB.LvlPrincipal)
                                        {
                                            lePlusNulEquipeB = perso;
                                        }
                                    }


                                    //On vérifie que les personnages les plus forts et les plus faibles existent pour éviter les NullReferenceException
                                    if (lePlusFortEquipeA != null && lePlusNulEquipeA != null && lePlusFortEquipeB != null && lePlusNulEquipeB != null)
                                    {
                                        equipe1.AjouterMembre(lePlusFortEquipeA);
                                        equipe2.AjouterMembre(lePlusNulEquipeB);
                                        equipe1.AjouterMembre(lePlusNulEquipeA);
                                        equipe2.AjouterMembre(lePlusFortEquipeB);

                                        //Ajouter le personnages si ce n'est pas le personnageA ou le personnageB ou les personnages les plus forts et les plus faibles de chaque équipe
                                        foreach (Personnage membreA in repEquipeA.Membres)
                                        {
                                            if (membreA != personnageA && membreA != lePlusFortEquipeA && membreA != lePlusNulEquipeA)
                                            {
                                                equipe1.AjouterMembre(membreA);
                                            }
                                        }

                                        //Ajouter le personnages si ce n'est pas le personnageA ou le personnageB ou les personnages les plus forts et les plus faibles de chaque équipe
                                        foreach (Personnage membreB in repEquipeB.Membres)
                                        {
                                            if (membreB != personnageB && membreB != lePlusFortEquipeB && membreB != lePlusNulEquipeB)
                                            {
                                                equipe2.AjouterMembre(membreB);
                                            }
                                        }

                                        //On vérifie que les deux équipes sont valides pour le problème SIMPLE avant de calculer le score
                                        if (equipe1.EstValide(Probleme.SIMPLE) && equipe2.EstValide(Probleme.SIMPLE))
                                        {
                                            double scoreNouvellesEquipes = 0;
                                            scoreNouvellesEquipes += equipe1.Score(Probleme.SIMPLE);
                                            scoreNouvellesEquipes += equipe2.Score(Probleme.SIMPLE);
                                            double scoreAvant = repEquipeA.Score(Probleme.SIMPLE) + repEquipeB.Score(Probleme.SIMPLE);
                                            double differenceScore = scoreNouvellesEquipes - scoreAvant;

                                            //On vérifie si la différence de score est négative pour savoir si le nouveau score est inferieur à celui de base
                                            if (differenceScore < 0)
                                            {
                                                Repartition nouvelleRep = new Repartition(jeuTest);
                                                nouvelleRep.AjouterEquipe(equipe1);
                                                nouvelleRep.AjouterEquipe(equipe2);
                                                for (int k = 0; k < repInitial.Equipes.Length; k++)
                                                {
                                                    if (repInitial.Equipes[k] != repEquipeA && repInitial.Equipes[k] != repEquipeB)
                                                    {
                                                        nouvelleRep.AjouterEquipe(repInitial.Equipes[k]);
                                                    }
                                                }

                                                //On ajoute les personnages restants
                                                repSwap = nouvelleRep;
                                                meilleur = true;
                                            }
                                        }

                                    }
                                }
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
            return repFinale;
        }

    }
}
