using System.Diagnostics;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class N_SwapNiveau2 : Algorithme
    {
        /// <summary>
        /// Algorithme de répartition de type N-Swap niveau 2.
        /// </summary>
        /// <param name="jeuTest"></param>
        /// <returns></returns>
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
                                    //Initalisation des niveau des différents rôles de deux équipes
                                    Personnage? tank1 = null;
                                    Personnage? tank2 = null;

                                    Personnage? support1 = null;
                                    Personnage? support2 = null;

                                    Personnage? dps1equipe1 = null;
                                    Personnage? dps2equipe1 = null;

                                    Personnage? dps1equipe2 = null;
                                    Personnage? dps2equipe2 = null;

                                    //On parcourt les membres des équipes pour trouver les personnages les plus forts et les plus faibles
                                    foreach (Personnage perso in repEquipeA.Membres)
                                    {
                                        if (perso.RolePrincipal == Role.TANK && tank1 == null)
                                        {
                                            tank1 = perso;
                                        }
                                        else if (perso.RolePrincipal == Role.SUPPORT && support1 == null)
                                        {
                                            support1 = perso;
                                        }
                                        else if (perso.RolePrincipal == Role.DPS)
                                        {
                                            if (dps1equipe1 == null)
                                            {
                                                dps1equipe1 = perso;
                                            }
                                            else if (dps2equipe1 == null)
                                            {
                                                dps2equipe1 = perso;
                                            }
                                        }
                                    }

                                    foreach (Personnage perso in repEquipeB.Membres)
                                    {
                                        if (perso.RolePrincipal == Role.TANK && tank2 == null)
                                        {
                                            tank2 = perso;
                                        }
                                        else if (perso.RolePrincipal == Role.SUPPORT && support2 == null)
                                        {
                                            support2 = perso;
                                        }
                                        else if (perso.RolePrincipal == Role.DPS)
                                        {
                                            if (dps1equipe2 == null)
                                            {
                                                dps1equipe2 = perso;
                                            }
                                            else if (dps2equipe2 == null)
                                            {
                                                dps2equipe2 = perso;
                                            }
                                        }
                                    }


                                    //Vérifie que tous les rôles sont bien assignés avant de continuer
                                    if (tank1 != null && tank2 != null && support1 != null && support2 != null && dps1equipe1 != null && dps2equipe1 != null && dps1equipe2 != null && dps2equipe2 != null)
                                    {
                                        //On ajoute les personnages dans les différentes équipes
                                        equipe1.AjouterMembre(tank1);
                                        equipe1.AjouterMembre(support1);
                                        equipe1.AjouterMembre(dps1equipe1);
                                        equipe1.AjouterMembre(dps2equipe1);

                                        equipe2.AjouterMembre(tank2);
                                        equipe2.AjouterMembre(support2);
                                        equipe2.AjouterMembre(dps1equipe2);
                                        equipe2.AjouterMembre(dps2equipe2);

                                        //Ajouter le personnages si ce n'est pas le personnageA ou le personnageB ou les personnages les plus forts et les plus faibles de chaque équipe
                                        foreach (Personnage membreA in repEquipeA.Membres)
                                        {
                                            if (membreA != personnageA && membreA != tank1 && membreA != support1 && membreA != dps1equipe1 && membreA != dps2equipe1)
                                            {
                                                equipe1.AjouterMembre(membreA);
                                            }
                                        }

                                        //Ajouter le personnages si ce n'est pas le personnageA ou le personnageB ou les personnages les plus forts et les plus faibles de chaque équipe
                                        foreach (Personnage membreB in repEquipeB.Membres)
                                        {
                                            if (membreB != personnageB && membreB != tank2 && membreB != support2 && membreB != dps1equipe2 && membreB != dps2equipe2)
                                            {
                                                equipe2.AjouterMembre(membreB);
                                            }
                                        }

                                        //On vérifie que les deux équipes sont valides pour le problème ROLEPRINCIPAL avant de calculer le score
                                        if (equipe1.EstValide(Probleme.ROLEPRINCIPAL) && equipe2.EstValide(Probleme.ROLEPRINCIPAL))
                                        {
                                            double scoreNouvellesEquipes = 0;
                                            scoreNouvellesEquipes += equipe1.Score(Probleme.ROLEPRINCIPAL);
                                            scoreNouvellesEquipes += equipe2.Score(Probleme.ROLEPRINCIPAL);
                                            double scoreAvant = repEquipeA.Score(Probleme.ROLEPRINCIPAL) + repEquipeB.Score(Probleme.ROLEPRINCIPAL);
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
