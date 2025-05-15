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
    public class n_swap : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            AlgorithmeGloutonCroissant algoGlouton = new AlgorithmeGloutonCroissant(); //met en place l'algorithme glouton croissant
            Repartition repartitioncourante = algoGlouton.Repartir(jeuTest); //repartition1 est la répartition de l'algorithme glouton croissant
            Repartition repfinal = new Repartition(jeuTest); //repartition2 est la répartition de l'algorithme n-swap

            // inverser les premiers membres de 2 équipes
            // Vérifier qu'il y a au moins 2 équipes et au moins 1 membre par équipe
            Equipe equipe = new Equipe();

                Equipe equipe1 = repartitioncourante.Equipes[0];
                Equipe equipe2 = repartitioncourante.Equipes[1];
            
                Personnage temp = equipe1.Membres[1];
                equipe1.Membres[1] = equipe2.Membres[1];
                equipe2.Membres[1] = temp;

          
            repfinal.AjouterEquipe(equipe1);
            repfinal.AjouterEquipe(equipe2);
            

            return repfinal;
        }


    }

}
