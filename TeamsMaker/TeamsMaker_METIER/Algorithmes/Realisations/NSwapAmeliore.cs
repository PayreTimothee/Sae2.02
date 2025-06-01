using System.Diagnostics;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class NSwapAmeliore : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Extreme_en_premier algoStart = new Extreme_en_premier();
            Repartition repInitial = algoStart.Repartir(jeuTest);
            Repartition repSwap = repInitial;
            bool meilleur = true;

            while (meilleur )
            {
                meilleur = false;
                bool ameliorationTrouvee = false;
                for (int i = 0; i < repSwap.Equipes.Length; i++)
                {
                    for (int j = i + 1; j < repSwap.Equipes.Length && ameliorationTrouvee != true; j++)
                    {
                        Equipe repEquipeA = repSwap.Equipes[i];
                        Equipe repEquipeB = repSwap.Equipes[j];
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

                                //Initalisation des personnages les plus forts et les plus faibles de chaque équipe.
                                Personnage? lePlusFortEquipeA = new Personnage(Classe.WARLOCK, 0, 0);
                                Personnage? lePlusNulEquipeA = new Personnage(Classe.WARLOCK, 100, 0);

                                Personnage? lePlusFortEquipeB = new Personnage(Classe.WARLOCK, 0, 0);
                                Personnage? lePlusNulEquipeB = new Personnage(Classe.WARLOCK, 100, 0);

                                //On parcourt les membres des équipes pour trouver les personnages les plus forts et les plus faibles
                                foreach (Personnage perso in repEquipeA.Membres)
                                {
                                    if (perso.LvlPrincipal >= lePlusFortEquipeA?.LvlPrincipal)
                                    {
                                        lePlusFortEquipeA = perso;
                                    }

                                    if (perso.LvlPrincipal <= lePlusNulEquipeA?.LvlPrincipal)
                                    {
                                        lePlusNulEquipeA = perso;
                                    }
                                }

                                foreach (Personnage perso in repEquipeB.Membres)
                                {
                                    if (perso.LvlPrincipal >= lePlusFortEquipeB?.LvlPrincipal)
                                    {
                                        lePlusFortEquipeB = perso;
                                    }

                                    if (perso.LvlPrincipal <= lePlusNulEquipeB?.LvlPrincipal)
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