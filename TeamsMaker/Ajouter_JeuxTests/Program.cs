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
            string[] ficher = new string[100];
            try
            {
                //On choisi le chemin de ou va aller le nouveau fichier, ici je l'appelle 'DixMille.jt'
                StreamWriter sw = new StreamWriter("C:\\Users\\payre\\Documents\\GitHub\\Sae2.02\\TeamsMaker\\TeamsMaker_METIER\\JeuxTest\\Fichier\\Test2.jt");

                //On initialise un compteur à 0, il n'est pas utilisé dans ce code mais peut être utile pour d'autres modifications
                int compteur = 0;

                //On choisi de faire 100 personnages
                for (int i = 1; i < 100; i++)
                {
                    compteur++;
                    //récupère toutes les valeurs de l'enum Classe et les stocke dans un tableau
                    Array values = Enum.GetValues(typeof(Classe));
                    Random random = new Random();

                    //Choisi une classe (provenant de value) aléatoire (tank, dps etc)
                    Classe randomBar = (Classe)values.GetValue(random.Next(values.Length));
                    
                    if (compteur < 25)
                    {
                        //On ajoute dans le fichier un rôle random, un niveau principal random allant de 1 à 100, de même pour le rôle secondaire allant de 1 à 100
                        ficher[i] = $"{randomBar} {random.Next(1, 39)} {random.Next(1, 100)}";
                    }

                    if (compteur > 25 && compteur < 75)
                    {
                        //On ajoute dans le fichier un rôle random, un niveau principal random allant de 1 à 100, de même pour le rôle secondaire allant de 1 à 100
                        ficher[i] = $"{randomBar} {random.Next(39, 69)} {random.Next(1, 100)}";
                    }

                    if (compteur > 75 && compteur < 100)
                    {
                        //On ajoute dans le fichier un rôle random, un niveau principal random allant de 1 à 100, de même pour le rôle secondaire allant de 1 à 100
                        ficher[i] = $"{randomBar} {random.Next(70, 100)} {random.Next(1, 100)}";
                    }
                    

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