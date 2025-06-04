using System.Diagnostics;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class NSwapAmeliore : Algorithme
    {
        ///<author> LAMBERT Hugo</author>
        /// <summary>
        /// Algorithme N-Swap Amélioré sur le principe de l'algorithme Extreme en premier.
        /// </summary>
        /// <param name="jeuTest"> Jeu de test utilisé </param>
        /// <returns> Repartition contenant les équipes de 4 personnages modifié </returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Extreme_en_premier algoStart = new Extreme_en_premier();
            Repartition repInitial = algoStart.Repartir(jeuTest);

            //Initialisation de l'index de l'equipe ayant le score le plus mauvais
            double EquipeNul = 0;

            //Initialisation de l'index de l'equipe ayant le score le plus haut
            double EquipeForte = 1;

            //On initialise la répartition de swap avec la répartition initiale
            Repartition repSwap = repInitial;
            bool meilleur = true;

            //On continue tant qu'on trouve une amélioration
            while (meilleur)
            {
                meilleur = false;
                bool ameliorationTrouvee = false;

                //On parcourt les équipes pour trouver l'index de l'équipe la plus faible et la plus forte
                for (int i = 0; i < repInitial.Equipes.Length; i++)
                {
                    if (repInitial.Equipes[i].Score(Probleme.SIMPLE) > EquipeNul)
                    {
                        EquipeNul = i;
                    }
                    if (repInitial.Equipes[i].Score(Probleme.SIMPLE) < EquipeForte)
                    {
                        EquipeForte = i;
                    }

                    //Initalisation des personnes le plus nul et le plus fort
                    Personnage? PlusFort = null;
                    Personnage? PlusNul = null;

                    // On parcourt les membres des équipes pour trouver le plus fort et le plus nul
                    foreach (Personnage perso in repInitial.Equipes[(int)EquipeForte].Membres)
                    {
                        if (PlusFort == null || perso.LvlPrincipal > PlusFort.LvlPrincipal)
                        {
                            PlusFort = perso;
                        }
                    }

                    // On parcourt les membres des équipes pour trouver le plus nul
                    foreach (Personnage perso in repInitial.Equipes[(int)EquipeNul].Membres)
                    {
                        if (PlusNul == null || perso.LvlPrincipal > PlusNul.LvlPrincipal)
                        {
                            PlusNul = perso;
                        }
                    }

                    if (repInitial.Equipes[(int)EquipeNul].Score(Probleme.SIMPLE) > 0.2 && repInitial.Equipes[(int)EquipeForte].Score(Probleme.SIMPLE) > 0.2)
                    {
                        //On parcours la liste qui a le score le plus faible
                        foreach (Personnage personnageRemplacer in repInitial.Equipes[(int)EquipeNul].Membres)
                        {
                            if (PlusFort != null && PlusNul != null)
                            {
                                Equipe equipe1 = new Equipe();
                                Equipe equipe2 = new Equipe();
                                equipe1.AjouterMembre(PlusFort);
                                equipe2.AjouterMembre(PlusNul);


                                foreach (Personnage membreA in repInitial.Equipes[(int)EquipeNul].Membres)
                                {
                                    if (membreA != PlusNul && membreA != PlusFort)
                                    {
                                        equipe1.AjouterMembre(membreA);
                                    }
                                }

                                foreach (Personnage membreB in repInitial.Equipes[(int)EquipeForte].Membres)
                                {
                                    if (membreB != PlusFort && membreB != PlusNul)
                                    {
                                        equipe2.AjouterMembre(membreB);
                                    }
                                }

                                if (equipe1.EstValide(Probleme.SIMPLE) && equipe2.EstValide(Probleme.SIMPLE))
                                {
                                    double scoreNouvellesEquipes = equipe1.Score(Probleme.SIMPLE) + equipe2.Score(Probleme.SIMPLE);
                                    double scoreAvant = repInitial.Equipes[(int)EquipeNul].Score(Probleme.SIMPLE) + repInitial.Equipes[(int)EquipeForte].Score(Probleme.SIMPLE);
                                    double differenceScore = scoreNouvellesEquipes - scoreAvant;
                                    if (differenceScore < 0)
                                    {
                                        Repartition nouvelleRep = new Repartition(jeuTest);
                                        nouvelleRep.AjouterEquipe(equipe1);
                                        nouvelleRep.AjouterEquipe(equipe2);
                                        for (int k = 0; k < repSwap.Equipes.Length; k++)
                                        {
                                            if (repSwap.Equipes[k] != repInitial.Equipes[(int)EquipeNul] && repSwap.Equipes[k] != repInitial.Equipes[(int)EquipeForte])
                                            {
                                                nouvelleRep.AjouterEquipe(repSwap.Equipes[k]);
                                            }
                                        }

                                        repSwap = nouvelleRep;
                                        meilleur = true;
                                        ameliorationTrouvee = true;
                                    }
                                }
                            }
                        }
                    }


                }
            }

            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            return repSwap;
        }
    }

}

