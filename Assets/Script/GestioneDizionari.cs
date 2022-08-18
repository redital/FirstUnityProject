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
            Dictionary<string, string> dizionario = new Dictionary<string, string>();

            using (var sr = new StreamReader(nomeFile))
            {
                while (sr.Peek() >= 0)
                {
                    string line=sr.ReadLine();
                    try
                    {
                        dizionario.Add(line.Split("=")[0] , line.Split("=")[1]);
                    }
                    catch (System.Exception)
                    {
                        Debug.Log("C'è una riga extra: " + '"' + line +'"');
                    }
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

        public static List<Skill> LetturaListaSkill(string nomeFile){
            //Dictionary<string, string> dizionario = new Dictionary<string, string>();
            List<Skill> skillList = new List<Skill>();

            using (var sr = new StreamReader(nomeFile))
            {
                while (sr.Peek() >= 0)
                {
                    string[] lines = new string[7];
                    for (int i = 0; i < 7; i++)
                    {
                        lines[i]=sr.ReadLine();
                    }
                    Skill skill = new Skill{
                        name=lines[0].Split("=")[1],
                        ATKMultiplier=float.Parse(lines[1].Split("=")[1]),
                        DEFMultiplier=float.Parse(lines[2].Split("=")[1]),
                        skillDuration=float.Parse(lines[3].Split("=")[1])/60.0f,  //non credo sia la soluzione migliore ma è l'unica che mi viene in mente
                        PAConsumati=int.Parse(lines[4].Split("=")[1]),
                        sprite=Resources.Load("IconeAbilità/"+lines[5].Split("=")[1]) as Sprite
                    };
                    try
                    {
                        skillList.Add(skill);
                    }
                    catch (System.Exception)
                    {
                        Debug.Log("non sono riuscito ad aggiungere " + lines);
                    }
                }
            }
            return skillList;
        }

        public static List<Item> LetturaListaItems(string nomeFile){
            //Dictionary<string, string> dizionario = new Dictionary<string, string>();
            List<Item> itemList = new List<Item>();

            using (var sr = new StreamReader(nomeFile))
            {
                while (sr.Peek() >= 0)
                {
                    string[] lines = new string[5];
                    for (int i = 0; i < 5; i++)
                    {
                        lines[i]=sr.ReadLine();
                    }
                    Item item = new Item{
                        name=lines[0].Split("=")[1],
                        quantità=int.Parse(lines[1].Split("=")[1]),
                        spriteName=lines[2].Split("=")[1],
                        isStackable=lines[3].Split("=")[1]=="true"
                    };
                    try
                    {
                        itemList.Add(item);
                    }
                    catch (System.Exception)
                    {
                        Debug.Log("non sono riuscito ad aggiungere " + lines);
                    }
                }
            }
            return itemList;
        }

    }

}

