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
    public class AlgorithmeGloutonDecroissant : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());
            Repartition repartition = new Repartition(jeuTest);
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = personnages.Length; i >= 4; i -= 4)
            {

                Equipe equipe = new Equipe();
                for (int j = i - 4 ; j < i ; j++)
                {
                    equipe.AjouterMembre(personnages[j]);

                }
                repartition.AjouterEquipe(equipe);
            }
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();

            return repartition;

        }
    }
}
