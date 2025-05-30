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
using Xceed.Wpf.Toolkit;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgorithmeNoperation : Algorithme
    {
        private int n;
        private int max;
        public AlgorithmeNoperation(int n, int max)
        {
            this.n = n;
            this.max = max;
        }

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Repartition meilleureRepartition = new Extreme_en_premier().Repartir(jeuTest);
            meilleureRepartition.LancerEvaluation(new Probleme());
            //Console.writeLine("Score de extreme : "+meilleureRepartition.Score);

            List<Equipe> equipes = meilleureRepartition.Equipes.ToList();
            int nombreEquipe = equipes.Count;

            if (this.n > nombreEquipe)
            {
                //Console.writeLine($"Impossible de reformer {this.n} équipe(s) parmi {nombreEquipe} existantes.");
                return meilleureRepartition;
            }

            var combinaisons = GetCombinations(equipes, this.n);
            meilleureRepartition.LancerEvaluation(new Probleme());
            double meilleurScore = meilleureRepartition.Score;
            Boolean estMeilleur =  true;
            while( estMeilleur )
            {
                estMeilleur = false;
                int nbcombinaisons = 0;
                foreach (List<Equipe> combinaison in combinaisons)
                {
                    nbcombinaisons++;
                    Console.WriteLine(nbcombinaisons);
                    List<Equipe> equipesModifiees = combinaison.ToList();
                    // 1. Obtenir tous les candidats
                    List<Personnage> candidats = combinaison.SelectMany(e => e.Membres).ToList();

                    // 2. Reformater les équipes
                    List<Equipe> nouvellesEquipes = FormerEquipesEquilibrees(candidats, this.n);
                    
                    // 3. Créer la nouvelle répartition
                    Repartition repartitionCandidate = new Repartition(jeuTest);
                    List<Equipe> equipesExistantes = meilleureRepartition.Equipes.ToList();

                    foreach (Equipe e in equipesExistantes)
                    {
                        if (!equipesModifiees.Contains(e)) // référence identique => ok
                        {
                            repartitionCandidate.AjouterEquipe(e);
                        }
                    }
                    //Console.writeLine("--------------------------------------------------------------équipe avant");
                    foreach (Equipe e in equipesModifiees)
                    {
                        //Console.writeLine("--------------------------------------------------------------");
                        int somme = 0;
                        foreach (Personnage personnage in e.Membres)
                        {
                            somme += personnage.LvlPrincipal;
                            //Console.writeLine($"{personnage.RolePrincipal} : {personnage.LvlPrincipal}");
                        }
                        //Console.writeLine("--------------------------------------------------------------" +somme/4);
                    }

                    foreach (Equipe e in nouvellesEquipes)
                        repartitionCandidate.AjouterEquipe(e);
                    //Console.writeLine("--------------------------------------------------------------éqipes modifiée");
                    foreach (Equipe e in nouvellesEquipes)
                    {
                        //Console.writeLine("--------------------------------------------------------------");
                        int somme = 0;
                        foreach (Personnage personnage in e.Membres)
                        {
                            somme += personnage.LvlPrincipal;
                            //Console.writeLine($"{personnage.RolePrincipal} : {personnage.LvlPrincipal}");
                        }
                        //Console.writeLine("--------------------------------------------------------------" + somme / 4);
                    }
                    repartitionCandidate.LancerEvaluation(new Probleme());
                    //Console.writeLine("Score voisin : " + repartitionCandidate.Score);


                    // 4. Comparer les scores
                    double nouveauScore = repartitionCandidate.Score;
                    if (nouveauScore < meilleurScore)
                    {
                        meilleureRepartition = repartitionCandidate;
                        meilleurScore = nouveauScore;
                        estMeilleur = true;
                        //Console.writeLine("------------------------------------------------------------------");
                    }
                }
            }

            stopwatch.Stop();
            this.TempsExecution=stopwatch.ElapsedMilliseconds;
            return meilleureRepartition;
        }


        private List<Equipe> FormerEquipesEquilibrees(List<Personnage> tousLesCandidats, int nombreEquipes)
        {
            List<Personnage> candidatsRestants = new List<Personnage>(tousLesCandidats);
            List<Equipe> equipes = new List<Equipe>();

            for (int i = 0; i < nombreEquipes; i++)
            {
                Equipe equipe = new Equipe();

                while (equipe.Membres.Count() < 4 && candidatsRestants.Count > 0)
                {
                    Personnage meilleurCandidat = null;
                    double ecartMin = double.MaxValue;

                    foreach (var candidat in candidatsRestants)
                    {
                        // Simulation moyenne avec ce candidat
                        var moyenneActuelle = equipe.Membres.Count() == 0 ? 0 : equipe.Membres.Average(p => p.LvlPrincipal);
                        var nouvelleMoyenne = (moyenneActuelle * equipe.Membres.Count() + candidat.LvlPrincipal) / (equipe.Membres.Count() + 1);
                        var ecart = Math.Abs(50 - nouvelleMoyenne);

                        if (ecart < ecartMin)
                        {
                            ecartMin = ecart;
                            meilleurCandidat = candidat;
                        }
                    }

                    if (meilleurCandidat != null)
                    {
                        equipe.AjouterMembre(meilleurCandidat);
                        candidatsRestants.Remove(meilleurCandidat);
                    }
                }

                equipes.Add(equipe);
            }

            // Affichage final
            ////Console.writeLine("--- Composition des équipes ---");
            for (int i = 0; i < equipes.Count; i++)
            {
                var moyenne = equipes[i].Membres.Average(p => p.LvlPrincipal);
                ////Console.writeLine($"Équipe {i} (moyenne {moyenne:F2}) :");
                foreach (var m in equipes[i].Membres)
                {
                    ////Console.writeLine($" - {m.RolePrincipal} ({m.LvlPrincipal})");
                }
            }

            
            return equipes;
        }





        // Méthode utilitaire pour générer les combinaisons de k éléments dans une liste
        private IEnumerable<List<T>> GetCombinations<T>(List<T> list, int k)
        {
            if (k == 0)
            {
                yield return new List<T>();
            }
            else
            {
                for (int i = 0; i <= list.Count - k; i++)
                {
                    var rest = list.Skip(i + 1).ToList();
                    foreach (var combinaison in GetCombinations(rest, k - 1))
                    {
                        combinaison.Insert(0, list[i]);
                        yield return combinaison;
                    }
                }
            }
        }


    }

}
