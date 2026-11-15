using System;
using System.Collections.Generic;

class Sekolah
{
    public string idSekolah;
    public string namaSekolah;
    public int jumlahPaketDiterima;

    public Sekolah(string id, string nama)
    {
        idSekolah = id;
        namaSekolah = nama;
        jumlahPaketDiterima = 0;
    }

    public void TampilInfo()
    {
        Console.WriteLine($"ID Sekolah     : {idSekolah}");
        Console.WriteLine($"Nama Sekolah   : {namaSekolah}");
        Console.WriteLine($"Paket Diterima : {jumlahPaketDiterima}");
    }
}

abstract class PaketMBG
{
    private string idPaket;
    private string namaPaket;
    private int stokDapur;

    public string IdPaket
    {
        get { return idPaket; }
        set { idPaket = value; }
    }

    public string NamaPaket
    {
        get { return namaPaket; }
        set { namaPaket = value; }
    }

    public int StokDapur
    {
        get { return stokDapur; }
        private set
        {
            if (value < 0)
            {
                Console.WriteLine("Stok tidak boleh negatif!");
                return;
            }
            stokDapur = value;
        }
    }

    public PaketMBG(string id, string nama, int stok)
    {
        idPaket = id;
        namaPaket = nama;
        StokDapur = stok;
    }

    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0)
        {
            Console.WriteLine("Jumlah pengurangan harus lebih dari 0");
            return false;
        }
        if (jumlah > stokDapur)
        {
            Console.WriteLine("Jumlah pengurangan melebihi stok yang tersedia");
            return false;
        }
        StokDapur = stokDapur - jumlah;
        return true;
    }

    public abstract int HitungBiaya(int jumlah);
}

class PaketReguler : PaketMBG
{
    public PaketReguler(string id, string nama, int stok) : base(id, nama, stok) { }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * 15000;
    }
}

class PaketKhusus : PaketMBG
{
    public string jenisDiet;

    public PaketKhusus(string id, string nama, int stok, string diet) : base(id, nama, stok)
    {
        jenisDiet = diet;
    }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * 20000;
    }
}

interface DapatDidistribusikan
{
    void KirimPaket();
}

class DistribusiMBG : DapatDidistribusikan
{
    private Sekolah sekolahTujuan;
    private PaketMBG paket;
    private int jumlah;

    public DistribusiMBG(Sekolah sekolah, PaketMBG paket, int jumlah)
    {
        this.sekolahTujuan = sekolah;
        this.paket = paket;
        this.jumlah = jumlah;
    }

    public void KirimPaket()
    {
        if (jumlah <= 0)
        {
            Console.WriteLine("Distribusi gagal: jumlah paket harus lebih dari 0");
            return;
        }

        if (jumlah > paket.StokDapur)
        {
            Console.WriteLine($"Distribusi gagal: jumlah paket ({jumlah}) melebihi stok dapur ({paket.StokDapur})");
            return;
        }

        paket.KurangiStok(jumlah);
        sekolahTujuan.jumlahPaketDiterima += jumlah;

        Console.WriteLine($"Distribusi MBG berhasil sebanyak {jumlah} paket makanan telah dikirim ke sekolah {sekolahTujuan.namaSekolah}");
    }
}

class PengelolaMBG
{
    private List<Sekolah> daftarSekolah = new List<Sekolah>();
    private List<PaketMBG> daftarPaket = new List<PaketMBG>();

    public void TambahSekolah(Sekolah sekolah)
    {
        daftarSekolah.Add(sekolah);
    }

    public void TambahPaket(PaketMBG paket)
    {
        daftarPaket.Add(paket);
    }

    public void TampilkanDataMBG()
    {
        Console.WriteLine("DATA SEKOLAH PENERIMA");
        foreach (Sekolah s in daftarSekolah)
        {
            s.TampilInfo();
            Console.WriteLine();
        }

        Console.WriteLine("DATA PAKET MBG");
        foreach (PaketMBG p in daftarPaket)
        {
            Console.WriteLine($"ID Paket : {p.IdPaket}");
            Console.WriteLine($"Nama : {p.NamaPaket}");
            Console.WriteLine($"Stok : {p.StokDapur}");
            if (p is PaketKhusus)
            {
                Console.WriteLine($"Diet : {((PaketKhusus)p).jenisDiet}");
            }
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main()
    {
        Sekolah s1 = new Sekolah("S01", "SDN 1 Jember");
        Sekolah s2 = new Sekolah("S02", "SMPN 2 Jember");
        Console.WriteLine("\nSoal 1: Info Sekolah");
        s1.TampilInfo();
        Console.WriteLine();
        s2.TampilInfo();

        PaketMBG reguler = new PaketReguler("P01", "Paket Reguler", 100);
        PaketMBG khusus = new PaketKhusus("P02", "Paket Khusus", 50, "Vegetarian");

        Console.WriteLine("\nSoal 4: Polymorphism (HitungBiaya)");
        PaketMBG[] semuaPaket = { reguler, khusus };
        foreach (PaketMBG p in semuaPaket)
        {
            Console.WriteLine($"{p.NamaPaket} x 10 = Rp{p.HitungBiaya(10)}");
        }

        Console.WriteLine("\nSoal 6: Distribusi");
        DapatDidistribusikan d1 = new DistribusiMBG(s1, reguler, 30);
        d1.KirimPaket();

        DapatDidistribusikan d2 = new DistribusiMBG(s2, khusus, 80);
        d2.KirimPaket();

        DapatDidistribusikan d3 = new DistribusiMBG(s2, khusus, 0);
        d3.KirimPaket();

        DapatDidistribusikan d4 = new DistribusiMBG(s2, khusus, 20);
        d4.KirimPaket();

        Console.WriteLine("\nSoal 7: Data MBG");
        PengelolaMBG pengelola = new PengelolaMBG();
        pengelola.TambahSekolah(s1);
        pengelola.TambahSekolah(s2);
        pengelola.TambahPaket(reguler);
        pengelola.TambahPaket(khusus);
        pengelola.TampilkanDataMBG();
    }
}