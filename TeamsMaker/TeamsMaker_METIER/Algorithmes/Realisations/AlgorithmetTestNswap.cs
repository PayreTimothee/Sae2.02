using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgorithmetTestNswap : Algorithme
    {
        /// <summary>
        /// Algo TITI
        /// </summary>
        /// <param name="jeuTest"></param>
        /// <returns></returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            AlgorithmeGloutonCroissant algoGlouton = new AlgorithmeGloutonCroissant();
            Repartition repGlouton = algoGlouton.Repartir(jeuTest);


            Repartition repFinale = new Repartition(jeuTest);

            Equipe equipe1 = new Equipe();
            equipe1.AjouterMembre(repGlouton.Equipes[1].Membres[0]); // On Ajoute premier member de l'autre équipe
            for (int i = 1; i < repGlouton.Equipes[0].Membres.Length - 1; i++)
            // !! bien int = 1 car le premier membre on l'a déjà swap
            // .....Length - 1 car on a déjà ajouté le premier membre ! ^^
            {
                equipe1.AjouterMembre(repGlouton.Equipes[0].Membres[i]);
            }
            foreach (Personnage perso in repGlouton.Equipes[0].Membres)
            {
                equipe1.AjouterMembre(perso);
            }

            Equipe equipe2 = new Equipe();
            equipe2.AjouterMembre(repGlouton.Equipes[0].Membres[0]);
            for (int i = 1; i < repGlouton.Equipes[1].Membres.Length - 1; i++)
            {
                equipe2.AjouterMembre(repGlouton.Equipes[1].Membres[i]);
            }
            foreach (Personnage perso in repGlouton.Equipes[1].Membres)
            {
                equipe2.AjouterMembre(perso);
            }

            repFinale.AjouterEquipe(equipe1);
            repFinale.AjouterEquipe(equipe2);

            return repFinale;
        }
    }

}