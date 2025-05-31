using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{

    public class N_Swap : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            Extreme_en_premier algoStart = new Extreme_en_premier();//initialiser l'algorithme de d�part
            Repartition repInitial = algoStart.Repartir(jeuTest);//initialise la repartition de d�part
            Repartition repSwap = repInitial;//mettre la repartition de d�part dans la variable de swap
            bool meilleur = true;//variable pour savoir si on a trouv� une meilleure repartition
            while (meilleur)//continue tant qu'il n'y a pas d'am�lioration trouv�e
            {
                meilleur = false;
                for (int i = 0; i < repInitial.Equipes.Length; i++)//parcour les diff�rentes paires
                {
                    for (int j = i + 1; j < repInitial.Equipes.Length; j++)
                    {
                        Equipe repEquipeA = repInitial.Equipes[i];
                        Equipe repEquipeB = repInitial.Equipes[j];
                        foreach (Personnage personnageA in repEquipeA.Membres)//parcours les membre de la premi�re �quipe
                        {
                            foreach (Personnage personnageB in repEquipeB.Membres)//parcours les membres de la deuxi�me �quipe
                            {
                                bool res = true;
                                //swap des membres entre les deux �quipes
                                Equipe equipe1 = new Equipe();
                                Equipe equipe2 = new Equipe();
                                equipe1.AjouterMembre(personnageB);
                                equipe2.AjouterMembre(personnageA);
                                foreach (Personnage membreA in repEquipeA.Membres)//ajoute les membres de la premi�re �quipe
                                {
                                    if (membreA != personnageA)
                                    {
                                        equipe1.AjouterMembre(membreA);
                                    }
                                }
                                foreach (Personnage membreB in repEquipeB.Membres)//ajoute les membres de la deuxi�me �quipe
                                {
                                    if (membreB != personnageB)
                                    {
                                        equipe2.AjouterMembre(membreB);
                                    }
                                }
                                res = res && equipe1.EstValide(Probleme.SIMPLE);//v�rifie si les �quipe sont valide
                                res = res && equipe2.EstValide(Probleme.SIMPLE);
                                if (res)//calcule le score des nouvelles �quipes
                                {
                                    double scoreNouvellesEquipes = 0;
                                    scoreNouvellesEquipes += equipe1.Score(Probleme.SIMPLE);
                                    scoreNouvellesEquipes += equipe2.Score(Probleme.SIMPLE);
                                    double scoreAvant = repEquipeA.Score(Probleme.SIMPLE) + repEquipeB.Score(Probleme.SIMPLE);
                                    double differenceScore = scoreNouvellesEquipes - scoreAvant;
                                    if (differenceScore < 0)//garde la r�partition si le score est meilleur
                                    {
                                        Repartition nouvelleRep = new Repartition(jeuTest);
                                        nouvelleRep.AjouterEquipe(equipe1);
                                        nouvelleRep.AjouterEquipe(equipe2);
                                        for (int k = 0; k < repInitial.Equipes.Length; k++)//ajoute les autres �quipes
                                        {
                                            if (repInitial.Equipes[k] != repEquipeA && repInitial.Equipes[k] != repEquipeB)
                                            {
                                                nouvelleRep.AjouterEquipe(repInitial.Equipes[k]);
                                            }
                                        }
                                        repSwap = nouvelleRep;
                                        meilleur = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Repartition repFinale = new Repartition(jeuTest);//ajoute les �quipes � la r�partition finale
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
