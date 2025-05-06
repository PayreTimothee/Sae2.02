using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class Extreme_en_premier : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());
            Repartition repartition = new Repartition(jeuTest);
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            int plus_fort = personnages.Length;
            int moins_fort = 0;
            for (int i = 0; i < personnages.Length - 4; i+=4)
            {

                Equipe equipe = new Equipe();
                for (int j = moins_fort; j < moins_fort + 2; j++) 
                {
                    equipe.AjouterMembre(personnages[j]);
                }
                for (int k = plus_fort - 1 ; k >= plus_fort - 2; k--)
                {
                    equipe.AjouterMembre(personnages[k]);
                }
                moins_fort = moins_fort + 2;
                plus_fort = plus_fort - 2;
                repartition.AjouterEquipe(equipe);
            }
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();

            return repartition;

        }

    }
}
