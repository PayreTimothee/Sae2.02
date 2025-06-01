using System.Diagnostics;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class N_Swap : Algorithme
    {
        /// <summary>
        /// Algorithme n-swap qui swap les personnages 
        /// </summary>
        /// <param name="jeuTest"> Jeu de test </param>
        /// <returns> Equipe de 4 personnages </returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Extreme_en_premier algoStart = new Extreme_en_premier();//initialiser l'algorithme de départ
            Repartition repInitial = algoStart.Repartir(jeuTest);//initialise la repartition de départ
            Repartition repSwap = repInitial;//mettre la repartition de départ dans la variable de swap
            bool meilleur = true;//variable pour savoir si on a trouvé une meilleure repartition

            while (meilleur)//continue tant qu'il n'y a pas d'amélioration trouvée
            {
                meilleur = false;
                bool ameliorementTrouvee = false;
                for (int i = 0; i < repSwap.Equipes.Length && !ameliorementTrouvee; i++)//parcour les différentes paires
                {
                    for (int j = i + 1; j < repSwap.Equipes.Length && !ameliorementTrouvee; j++)
                    {
                        Equipe repEquipeA = repSwap.Equipes[i];
                        Equipe repEquipeB = repSwap.Equipes[j];
                        int a = 0;
                        while (a < repEquipeA.Membres.Length && !ameliorementTrouvee)
                        {
                            Personnage personnageA = repEquipeA.Membres[a];
                            int b = 0;
                            while (b < repEquipeB.Membres.Length && !ameliorementTrouvee)
                            {
                                Personnage personnageB = repEquipeB.Membres[b];
                                //swap des membres entre les deux équipes
                                Equipe equipe1 = new Equipe();
                                Equipe equipe2 = new Equipe();
                                equipe1.AjouterMembre(personnageB);
                                equipe2.AjouterMembre(personnageA);
                                foreach (Personnage membreA in repEquipeA.Membres)
                                {
                                    if (membreA != personnageA)
                                    {
                                        equipe1.AjouterMembre(membreA);
                                    }
                                }
                                foreach (Personnage membreB in repEquipeB.Membres)
                                {
                                    if (membreB != personnageB)
                                    {
                                        equipe2.AjouterMembre(membreB);
                                    }
                                }
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
                                        ameliorementTrouvee = true;
                                    }
                                }
                                b++;// incrémente
                            }
                            a++;//incrémente
                        }
                    }
                }
            }
            Repartition repFinale = new Repartition(jeuTest);//ajoute les équipes à la répartition finale
            foreach (Equipe equipe in repSwap.Equipes)
            {
                if (equipe.Score(Probleme.SIMPLE) < 400)
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
