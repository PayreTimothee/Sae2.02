using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class N_SwapNiveau3 : Algorithme
    {
        ///<author> PAYRE Timothée </author>
        /// <summary>
        /// Algorithme n-swap qui swap les personnages 
        /// </summary>
        /// <param name="jeuTest"> Jeu de test </param>
        /// <returns> Répartition contenant les équipes de 4 personnages </returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Heuristique1_niveau2 algoStart = new Heuristique1_niveau2();//initialiser l'algorithme de départ

            Repartition repInitial = algoStart.Repartir(jeuTest);//initialise la repartition de départ
            Repartition repSwap = repInitial;//mettre la repartition de départ dans la variable de swap
            bool meilleur = true;//variable pour savoir si on a trouvé une meilleure repartition

            while (meilleur)//continue tant qu'il n'y a pas d'amélioration trouvée
            {
                meilleur = false;
                bool ameliorationTrouvee = false;
                for (int i = 0; i < repSwap.Equipes.Length && !ameliorationTrouvee; i++)//parcour les différentes paires
                {
                    for (int j = i + 1; j < repSwap.Equipes.Length && !ameliorationTrouvee; j++)
                    {
                        //On récupère les équipes A et B de la répartition de swap
                        Equipe repEquipeA = repSwap.Equipes[i];
                        Equipe repEquipeB = repSwap.Equipes[j];


                        int a = 0;

                        //On parcourt les membres de l'équipe A et on vérifie q
                        while (a < repEquipeA.Membres.Length && ameliorationTrouvee == false)
                        {
                            //On récupère le personnage A
                            Personnage personnageA = repEquipeA.Membres[a];

                            //On récupère le rôle du personnage A
                            Role rolePerso1 = repEquipeA.Membres[a].RolePrincipal;
                            int b = 0;
                            while (b < repEquipeB.Membres.Length && ameliorationTrouvee == false)
                            {
                                //On récupère le personnage B
                                Personnage personnageB = repEquipeB.Membres[b];

                                //On recupère le rôle du personnage B
                                Role rolePerso2 = repEquipeB.Membres[b].RolePrincipal;

                                //Création des équipes après le swap
                                Equipe equipe1 = new Equipe();
                                Equipe equipe2 = new Equipe();

                                //On Swap uniquement si les perosnnages ont le même rôle principal
                                if (rolePerso1 == rolePerso2)
                                {
                                    equipe1.AjouterMembre(personnageB);
                                    equipe2.AjouterMembre(personnageA);
                                }


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
                                if (equipe1.EstValide(Probleme.ROLESECONDAIRE) && equipe2.EstValide(Probleme.ROLESECONDAIRE))
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
