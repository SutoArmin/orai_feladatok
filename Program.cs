using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Esemeny
{
    public string Kod { get; set; }
    public int Perc { get; set; }
    public int Tipus { get; set; }
}

class Program
{
    static int IdoPercre(string ido)
    {
        var p = ido.Split(':');
        return int.Parse(p[0]) * 60 + int.Parse(p[1]);
    }

    static string PercIdo(int p)
    {
        return $"{p / 60:00}:{p % 60:00}";
    }

    static void Main()
    {
        // 1. feladat – beolvasás
        List<Esemeny> lista = new List<Esemeny>();

        foreach (var sor in File.ReadAllLines("bedat.txt"))
        {
            var t = sor.Split();
            lista.Add(new Esemeny
            {
                Kod = t[0],
                Perc = IdoPercre(t[1]),
                Tipus = int.Parse(t[2])
            });
        }

        // 2. feladat – első belépés, utolsó kilépés
        Console.WriteLine("2. feladat");

        var elso = lista.First(x => x.Tipus == 1);
        var utolso = lista.Last(x => x.Tipus == 2);

        Console.WriteLine($"Az első tanuló {PercIdo(elso.Perc)}-kor lépett be a főkapun.");
        Console.WriteLine($"Az utolsó tanuló {PercIdo(utolso.Perc)}-kor lépett ki a főkapun.");

        // 3. feladat – késők listája
        using (var sw = new StreamWriter("kesok.txt"))
        {
            foreach (var e in lista.Where(x => x.Tipus == 1 &&
                                               x.Perc > IdoPercre("07:50") &&
                                               x.Perc <= IdoPercre("08:15")))
            {
                sw.WriteLine($"{PercIdo(e.Perc)} {e.Kod}");
            }
        }

        // 4. feladat – menzán ebédelők száma
        Console.WriteLine("4. feladat");
        int menza = lista.Count(x => x.Tipus == 3);
        Console.WriteLine($"A menzán aznap {menza} tanuló ebédelt.");

        // 5. feladat – könyvtári kölcsönzők
        Console.WriteLine("5. feladat");

        var kolcsonzok = lista.Where(x => x.Tipus == 4)
                              .Select(x => x.Kod)
                              .Distinct()
                              .Count();

        Console.WriteLine($"Aznap {kolcsonzok} tanuló kölcsönzött a könyvtárban.");

        if (kolcsonzok > menza)
            Console.WriteLine("Többen voltak, mint a menzán.");
        else
            Console.WriteLine("Nem voltak többen, mint a menzán.");

        // 6. feladat – hátsó kapus eset (HashSet nélkül)
        Console.WriteLine("6. feladat");
        Console.WriteLine("Az érintett tanulók:");

        List<string> kiment = new List<string>();
        List<string> visszajott = new List<string>();

        foreach (var e in lista)
        {
            if (e.Perc >= IdoPercre("10:45") && e.Perc <= IdoPercre("10:50") && e.Tipus == 2)
                if (!kiment.Contains(e.Kod))
                    kiment.Add(e.Kod);

            if (e.Perc > IdoPercre("10:50") && e.Perc <= IdoPercre("11:00") && e.Tipus == 1)
                if (!visszajott.Contains(e.Kod))
                    visszajott.Add(e.Kod);
        }

        // közös elemek keresése lista-lista között
        foreach (var kod in kiment)
        {
            if (visszajott.Contains(kod))
                Console.Write(kod + " ");
        }

        Console.WriteLine();

        // 7. feladat – egy tanuló bent tartózkodási ideje
        Console.WriteLine("\n7. feladat");
        Console.Write("Egy tanuló azonosítója=");
        string keres = Console.ReadLine().Trim();

        var tanulo = lista.Where(x => x.Kod == keres).ToList();

        if (tanulo.Count == 0)
        {
            Console.WriteLine("Ilyen azonosítójú tanuló aznap nem volt az iskolában.");
        }
        else
        {
            int elsoBe = tanulo.First(x => x.Tipus == 1).Perc;
            int utolsoKi = tanulo.Last(x => x.Tipus == 2).Perc;

            int diff = utolsoKi - elsoBe;

            Console.WriteLine($"A tanuló érkezése és távozása között {diff / 60} óra {diff % 60} perc telt el.");
        }
    }
}
