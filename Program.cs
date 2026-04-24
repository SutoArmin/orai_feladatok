using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Jel
{
    public string Rendszam;
    public int Ora;
    public int Perc;
    public int Sebesseg;

    public Jel(string r, int o, int p, int s)
    {
        Rendszam = r;
        Ora = o;
        Perc = p;
        Sebesseg = s;
    }

    public int Percben() => Ora * 60 + Perc;
}

class Program
{
    static void Main()
    {
        // 1. feladat – beolvasás
        List<Jel> adatok = new List<Jel>();
        foreach (var sor in File.ReadAllLines("jeladas.txt"))
        {
            var t = sor.Split('\t');
            adatok.Add(new Jel(t[0], int.Parse(t[1]), int.Parse(t[2]), int.Parse(t[3])));
        }

        // 2. feladat
        Console.WriteLine("2. feladat:");
        var utolso = adatok.Last();
        Console.WriteLine($"Az utolso jeladas idopontja {utolso.Ora}:{utolso.Perc}, a jarmu rendszama {utolso.Rendszam}");

        // 3. feladat
        Console.WriteLine("3. feladat:");
        string elsoRsz = adatok[0].Rendszam;
        Console.WriteLine($"Az elso jarmu: {elsoRsz}");
        Console.Write("Jeladasainak idopontjai: ");

        var idopontok = adatok
            .Where(a => a.Rendszam == elsoRsz)
            .Select(a => $"{a.Ora}:{a.Perc}");

        Console.WriteLine(string.Join(" ", idopontok));

        // 4. feladat
        Console.WriteLine("4. feladat:");
        Console.Write("Kerem, adja meg az orat: ");
        int bekertOra = int.Parse(Console.ReadLine());
        Console.Write("Kerem, adja meg a percet: ");
        int bekertPerc = int.Parse(Console.ReadLine());

        int db = adatok.Count(a => a.Ora == bekertOra && a.Perc == bekertPerc);
        Console.WriteLine($"A jeladasok szama: {db}");

        // 5. feladat
        Console.WriteLine("5. feladat:");
        int maxSeb = adatok.Max(a => a.Sebesseg);
        Console.WriteLine($"A legnagyobb sebesseg km/h: {maxSeb}");
        Console.Write("A jarmuvek: ");

        var maxRendszamok = adatok
            .Where(a => a.Sebesseg == maxSeb)
            .Select(a => a.Rendszam);

        Console.WriteLine(string.Join(" ", maxRendszamok));

        // 6. feladat
        Console.WriteLine("6. feladat:");
        Console.Write("Kerem, adja meg a rendszamot: ");
        string keresett = Console.ReadLine().Trim();

        var auto = adatok.Where(a => a.Rendszam == keresett).ToList();

        if (auto.Count == 0)
        {
            Console.WriteLine("Nincs ilyen rendszamu jarmu az adatok kozott.");
        }
        else
        {
            double tav = 0.0;
            int elozoIdo = auto[0].Percben();
            int elozoSeb = auto[0].Sebesseg;

            foreach (var jel in auto)
            {
                int ido = jel.Percben();

                if (jel != auto.First())
                {
                    int eltelt = ido - elozoIdo;
                    tav += (eltelt / 60.0) * elozoSeb;
                }

                Console.WriteLine($"{jel.Ora}:{jel.Perc} {tav:F1} km");

                elozoIdo = ido;
                elozoSeb = jel.Sebesseg;
            }
        }

        // 7. feladat – ido.txt létrehozása
        var jarmuvek = new Dictionary<string, (int o1, int p1, int o2, int p2)>();

        foreach (var a in adatok)
        {
            if (!jarmuvek.ContainsKey(a.Rendszam))
                jarmuvek[a.Rendszam] = (a.Ora, a.Perc, a.Ora, a.Perc);
            else
                jarmuvek[a.Rendszam] = (jarmuvek[a.Rendszam].o1, jarmuvek[a.Rendszam].p1, a.Ora, a.Perc);
        }

        using (var f = new StreamWriter("ido.txt"))
        {
            foreach (var kv in jarmuvek)
            {
                f.WriteLine($"{kv.Key} {kv.Value.o1} {kv.Value.p1} {kv.Value.o2} {kv.Value.p2}");
            }
        }
    }
}
