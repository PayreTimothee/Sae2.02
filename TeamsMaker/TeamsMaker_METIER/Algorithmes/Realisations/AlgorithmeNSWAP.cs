using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgorithmeNswap : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            AlgorithmeGloutonCroissant algoGlouton = new AlgorithmeGloutonCroissant(); //met en place l'algorithme glouton croissant
            Repartition repartitioncourante = algoGlouton.Repartir(jeuTest); //repartition1 est la répartition de l'algorithme glouton croissant
           double scoreInitial = repartitioncourante.Score; //scoreInitial est le score de la répartition de l'algorithme glouton croissant
            Repartition repartitionfinal = new Repartition(jeuTest); //repartition2 est la répartition de l'algorithme n-swap

            bool amelioration = true; //amelioration est un booléen qui indique si on a trouvé une amélioration ou pas

            while (amelioration) //Tant qu'on a trouvé une amélioration
            {
                amelioration = false; //On initialise amelioration à faux
                for (int i = 0; i < repartitioncourante.Equipes.Count(); i++) //Pour chaque équipe de la répartition courante
                {
                    for (int j = 0; j < repartitioncourante.Equipes[i].Membres.Count(); j++) //Pour chaque membre de l'équipe
                    {
                        for (int k = 0; k < repartitioncourante.Equipes.Count(); k++) //Pour chaque équipe de la répartition courante
                        {
                            for (int l = 0; l < repartitioncourante.Equipes[k].Membres.Count(); l++) //Pour chaque membre de l'équipe
                            {
                                if (i != k && j != l) //Si ce n'est pas le même membre et pas la même équipe
                                {
                                    Personnage temp = repartitioncourante.Equipes[i].Membres[j]; //On stocke le membre dans une variable temporaire
                                    repartitioncourante.Equipes[i].Membres[j] = repartitioncourante.Equipes[k].Membres[l]; //On remplace le membre par un autre membre
                                    repartitioncourante.Equipes[k].Membres[l] = temp; //On remplace le membre par le membre temporaire
                                    if (repartitioncourante.Score > scoreInitial) //Si le score de la répartition courante est supérieur au score initial
                                    {
                                        amelioration = true; //On a trouvé une amélioration
                                        scoreInitial = repartitioncourante.Score; //On met à jour le score initial
                                    }
                                    else //Sinon on remet les membres à leur place
                                    {
                                        repartitioncourante.Equipes[i].Membres[j] = temp;
                                        repartitioncourante.Equipes[k].Membres[l] = repartitioncourante.Equipes[i].Membres[j];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            while (amelioration) //Tant qu'on a trouvé une amélioration
            {
                amelioration = false; //On initialise amelioration à faux

            }
                //Repartition repfinal = new Repartition(jeuTest); //repartition2 est la répartition de l'algorithme n-swap

                //Repartition scoreInitial = new Repartition.score(jeuTest); //repartition3 est la répartition de l'algorithme de score



                //// inverser les premiers membres de 2 équipes
                //// Vérifier qu'il y a au moins 2 équipes et au moins 1 membre par équipe
                //Equipe equipe = new Equipe();

                //    Equipe equipe1 = repartitioncourante.Equipes[0];
                //    Equipe equipe2 = repartitioncourante.Equipes[1];

                //    Personnage temp = equipe1.Membres[1];
                //    equipe1.Membres[1] = equipe2.Membres[1];
                //    equipe2.Membres[1] = temp;


                //repfinal.AjouterEquipe(equipe1);
                //repfinal.AjouterEquipe(equipe2);


                return repartitioncourante;
        }


    }

}
