using System;
using System.IO;

using System.Linq;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gestione          //Penso possa essere rimosso e fare tutto in maniera molto più semplice
{   
    /* Questa classe è pensata come semplice libreria per facilitare l'utilizzo dei dizionari.
        Utiliziamo i dizionari per memorizzare le informazioni in maniera compatta, ogni dizionario è legato tramite i metodi
        Salva e Carica del GameManager a dei file di testo, così da conservare le informazioni anche a gioco spento
        Utiliziamo formattati con una informazione per riga, codificata come <chiave>=<valore> (senza spazi)
    */

    public class GestioneDizionari
    {
        // Metodo che legge un file e trascrive le informazioni in un dizionario, restituisce il dizionario compilato
        public static Dictionary<string, string> LetturaDizionario(string nomeFile){
            string text = "";
            Dictionary<string, string> dizionario = new Dictionary<string, string>();

            using (var sr = new StreamReader(nomeFile))
            {
                // Read the stream as a string.
                text = sr.ReadToEnd();
            }
            
            string[] lines = text.Split('\n');

            foreach (string line in lines){
                try
                {
                    dizionario.Add(line.Split("=")[0],line.Split("=")[1]);
                }
                catch (System.Exception)
                {
                    Debug.Log("C'è una riga extra: " + '"' + line +'"');
                }
                    
            }
            return dizionario;
        }

        // Metodo che legge un dizionario e trascrive le informazioni su file
        public static void ScritturaDizionario(Dictionary<string, string> dizionario, string nomeFile){
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), nomeFile))){
                foreach (string key in dizionario.Keys){
                    string line=key + "=" + dizionario[key];
                    outputFile.WriteLine(line);
                }
            }
        }

        // Metodo che aggiorna o aggiunge un oggetto ud un dizionario ed aggiorna il file di testo (da valutare se ha senso tenerlo)
        public static void AggiornaCose(string chiave, string valore, string nomeFile){
                 
            Dictionary<string, string> dizionario =LetturaDizionario(nomeFile);
            
            if(dizionario.ContainsKey(chiave)){
                int nuovoValore=int.Parse(valore)+int.Parse(dizionario[chiave]);
                dizionario[chiave]=nuovoValore.ToString();
            }
            else{
                dizionario.Add(chiave,valore);
            }


            ScritturaDizionario(dizionario,nomeFile);
        }


        // Metodi specifici per inventario e statistiche
        public static void AggiornaInventario(string chiave, string valore){
            AggiornaCose(chiave, valore, "Inventario.txt");
        }

        public static void AggiornaStatistiche(string chiave, string valore){
            AggiornaCose(chiave, valore, "Statistiche.txt");
        }

    }

}

