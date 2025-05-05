using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages.Classes;

namespace TeamsMaker
{
    public class Program
    {
        static void Main(string[] args)
        {
            //J'ai regardé sur le site officiel de Microsoft afin d'écrire sur un fichier / nouveau fichier
            string[] ficher = new string[10000];
            try
            {
                //On choisi le chemin de ou va aller le nouveau fichier, ici je l'appelle 'DixMille.jt'
                StreamWriter sw = new StreamWriter("C:\\Users\\hugol\\Desktop\\Cours_iut\\Semestre2\\S2_02 - Exploration algorithmique d'un problème\\TP\\TeamsMaker\\TeamsMaker_METIER\\JeuxTest\\Fichiers\\DixMille.jt");
                
                //On choisi de faire 10 000 personnages
                for (int i = 1; i < 10000; i++)
                {
                    //récupère toutes les valeurs de l'enum Classe et les stocke dans un tableau
                    Array values = Enum.GetValues(typeof(Classe));
                    Random random = new Random();

                    //Choisi une classe (provenant de value) aléatoire (tank, dps etc)
                    Classe randomBar = (Classe)values.GetValue(random.Next(values.Length));

                    //On ajoute dans fichier un rôle random, un niveau principal random, de même pour le rôle secondaire
                    ficher[i] = $"{randomBar} {random.Next(1, 100)} {random.Next(1, 100)}";

                    //Ecrit dans le fichier fichier[i]
                    sw.WriteLine(ficher[i]);
                }
                sw.Close();
            }
            //Exception
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Le fichier a été créée avec succès");
            }
        }
    }
}