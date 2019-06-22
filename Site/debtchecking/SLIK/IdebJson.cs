using System;
using System.Collections.Generic;
using System.Data;
using DMS.Tools;

namespace DebtChecking.Facilities
{
    public class IdebJson
    {
        public class idebIndividu
        {
            public header header { get; set; }
            public individual individual { get; set; }
        }

        public class idebPerusahaan
        {
            public header header { get; set; }
            public perusahaan perusahaan { get; set; }
        }

        public class header
        {
            public string kodeReferensiPengguna { get; set; }
            public string tanggalHasil { get; set; }
            public string idPermintaan { get; set; }
            public string idPenggunaPermintaan { get; set; }
            public string dibuatOleh { get; set; }
            public string kodeLJKPermintaan { get; set; }
            public string kodeCabangPermintaan { get; set; }
            public string kodeTujuanPermintaan { get; set; }
            public string tanggalPermintaan { get; set; }
            public string totalBagian { get; set; }
            public string nomorBagian { get; set; }
        }
        
        public class individual
        {
            public string nomorLaporan { get; set; }
            public string posisiDataTerakhir { get; set; }
            public string tanggalPermintaan { get; set; }
            public parameterPencarian parameterPencarian { get; set; }
            public dataPokokDebitur[] dataPokokDebitur { get; set; }
            public ringkasanFasilitas ringkasanFasilitas { get; set; }
            public fasilitas fasilitas { get; set; }
        }

        public class parameterPencarian
        {
            public string namaDebitur { get; set; }
            public string jenisKelamin { get; set; }
            public string jenisKelaminKet { get; set; }
            public string noIdentitas { get; set; }
            public string npwp { get; set; }
            public string tempatLahir { get; set; }
            public string tanggalLahir { get; set; }
        }

        public class dataPokokDebitur
        {
            public string namaDebitur { get; set; }
            public string identitas { get; set; }
            public string noIdentitas { get; set; }
            public string alamat { get; set; }
            public string jenisKelamin { get; set; }
            public string jenisKelaminKet { get; set; }
            public string npwp { get; set; }
            public string tempatLahir { get; set; }
            public string tanggalLahir { get; set; }
            public string pelapor { get; set; }
            public string pelaporKet { get; set; }
            public string tanggalDibentuk { get; set; }
            public string tanggalUpdate { get; set; }
            public string kelurahan { get; set; }
            public string kecamatan { get; set; }
            public string kabKota { get; set; }
            public string kabKotaKet { get; set; }
            public string kodePos { get; set; }
            public string negara { get; set; }
            public string negaraKet { get; set; }
            public string pekerjaan { get; set; }
            public string pekerjaanKet { get; set; }
            public string tempatBekerja { get; set; }
            public string bidangUsaha { get; set; }
            public string bidangUsahaKet { get; set; }
            public string kodeGelarDebitur { get; set; }
            public string statusGelarDebitur { get; set; }
        }

        public class ringkasanFasilitas
        {
            public string plafonEfektifKreditPembiayaan { get; set; }
            public string plafonEfektifLc { get; set; }
            public string plafonEfektifGyd { get; set; }
            public string plafonEfektifSec { get; set; }
            public string plafonEfektifLain { get; set; }
            public string plafonEfektifTotal { get; set; }
            public string bakiDebetKreditPembiayaan { get; set; }
            public string bakiDebetLc { get; set; }
            public string bakiDebetGyd { get; set; }
            public string bakiDebetSec { get; set; }
            public string bakiDebetLain { get; set; }
            public string bakiDebetTotal { get; set; }
            public string krediturBankUmum { get; set; }
            public string krediturBPR { get; set; }
            public string krediturLp { get; set; }
            public string krediturLainnya { get; set; }
            public string kualitasTerburuk { get; set; }
            public string kualitasBulanDataTerburuk { get; set; }
        }

        public class fasilitas
        {
            public kreditPembiayan[] kreditPembiayan { get; set; }
        }

        public class kreditPembiayan
        {
            public string ljk { get; set; }
            public string ljkKet { get; set; }
            public string cabang { get; set; }
            public string cabangKet { get; set; }
            public string bakiDebet { get; set; }
            public string tanggalDibentuk { get; set; }
            public string tanggalUpdate { get; set; }
            public string bulan { get; set; }
            public string tahun { get; set; }
            public string sifatKreditPembiayaan { get; set; }
            public string sifatKreditPembiayaanKet { get; set; }
            public string jenisKreditPembiayaan { get; set; }
            public string jenisKreditPembiayaanKet { get; set; }
            public string akadKreditPembiayaan { get; set; }
            public string akadKreditPembiayaanKet { get; set; }
            public string noRekening { get; set; }
            public string frekPerpjganKreditPembiayaan { get; set; }
            public string noAkadAwal { get; set; }
            public string tanggalAkadAwal { get; set; }
            public string noAkadAkhir { get; set; }
            public string tanggalAkadAkhir { get; set; }
            public string tanggalAwalKredit { get; set; }
            public string tanggalMulai { get; set; }
            public string tanggalJatuhTempo { get; set; }
            public string kategoriDebiturKode { get; set; }
            public string kategoriDebiturKet { get; set; }
            public string jenisPenggunaan { get; set; }
            public string jenisPenggunaanKet { get; set; }
            public string sektorEkonomi { get; set; }
            public string sektorEkonomiKet { get; set; }
            public string kreditProgramPemerintah { get; set; }
            public string kreditProgramPemerintahKet { get; set; }
            public string lokasiProyek { get; set; }
            public string lokasiProyekKet { get; set; }
            public string valutaKode { get; set; }
            public string sukuBungaImbalan { get; set; }
            public string jenisSukuBungaImbalan { get; set; }
            public string jenisSukuBungaImbalanKet { get; set; }
            public string kualitas { get; set; }
            public string kualitasKet { get; set; }
            public string jumlahHariTunggakan { get; set; }
            public string nilaiProyek { get; set; }
            public string plafonAwal { get; set; }
            public string plafon { get; set; }
            public string realisasiBulanBerjalan { get; set; }
            public string nilaiDalamMataUangAsal { get; set; }
            public string kodeSebabMacet { get; set; }
            public string sebabMacetKet { get; set; }
            public string tanggalMacet { get; set; }
            public string tunggakanPokok { get; set; }
            public string tunggakanBunga { get; set; }
            public string frekuensiTunggakan { get; set; }
            public string denda { get; set; }
            public string frekuensiRestrukturisasi { get; set; }
            public string tanggalRestrukturisasiAkhir { get; set; }
            public string kodeCaraRestrukturisasi { get; set; }
            public string restrukturisasiKet { get; set; }
            public string kondisi { get; set; }
            public string kondisiKet { get; set; }
            public string tanggalKondisi { get; set; }
            public string keterangan { get; set; }
            public string tahunBulan01Ht { get; set; }
            public string tahunBulan01 { get; set; }
            public string tahunBulan01Kol { get; set; }
            public string tahunBulan02Ht { get; set; }
            public string tahunBulan02 { get; set; }
            public string tahunBulan02Kol { get; set; }
            public string tahunBulan03Ht { get; set; }
            public string tahunBulan03 { get; set; }
            public string tahunBulan03Kol { get; set; }
            public string tahunBulan04Ht { get; set; }
            public string tahunBulan04 { get; set; }
            public string tahunBulan04Kol { get; set; }
            public string tahunBulan05Ht { get; set; }
            public string tahunBulan05 { get; set; }
            public string tahunBulan05Kol { get; set; }
            public string tahunBulan06Ht { get; set; }
            public string tahunBulan06 { get; set; }
            public string tahunBulan06Kol { get; set; }
            public string tahunBulan07Ht { get; set; }
            public string tahunBulan07 { get; set; }
            public string tahunBulan07Kol { get; set; }
            public string tahunBulan08Ht { get; set; }
            public string tahunBulan08 { get; set; }
            public string tahunBulan08Kol { get; set; }
            public string tahunBulan09Ht { get; set; }
            public string tahunBulan09 { get; set; }
            public string tahunBulan09Kol { get; set; }
            public string tahunBulan10Ht { get; set; }
            public string tahunBulan10 { get; set; }
            public string tahunBulan10Kol { get; set; }
            public string tahunBulan11Ht { get; set; }
            public string tahunBulan11 { get; set; }
            public string tahunBulan11Kol { get; set; }
            public string tahunBulan12Ht { get; set; }
            public string tahunBulan12 { get; set; }
            public string tahunBulan12Kol { get; set; }
            public agunan[] agunan { get; set; }
            public penjamin[] penjamin { get; set; }
        }

        public class agunan
        {
            public string jenisAgunanKet { get; set; }
            public string nilaiAgunanMenurutLJK { get; set; }
            public string prosentaseParipasu { get; set; }
            public string tanggalUpdate { get; set; }
            public string nomorAgunan { get; set; }
            public string jenisPengikatan { get; set; }
            public string jenisPengikatanKet { get; set; }
            public string tanggalPengikatan { get; set; }
            public string namaPemilikAgunan { get; set; }
            public string alamatAgunan { get; set; }
            public string kabKotaLokasiAgunan { get; set; }
            public string kabKotaLokasiAgunanKet { get; set; }
            public string tglPenilaianPelapor { get; set; }
            public string peringkatAgunan { get; set; }
            public string kodeLembagaPemeringkat { get; set; }
            public string lembagaPemeringkat { get; set; }
            public string buktiKepemilikan { get; set; }
            public string nilaiAgunanNjop { get; set; }
            public string nilaiAgunanIndep { get; set; }
            public string namaPenilaiIndep { get; set; }
            public string asuransi { get; set; }
            public string tanggalPenilaianPenilaiIndependen { get; set; }
            public string keterangan { get; set; }
        }

        public class penjamin
        {

        }

        public class perusahaan
        {
            public string nomorLaporan { get; set; }
            public string posisiDataTerakhir { get; set; }
            public string tanggalPermintaan { get; set; }
            public parameterPencarianPerusahaan parameterPencarian { get; set; }
            public dataPokokDebiturPerusahaan[] dataPokokDebitur { get; set; }
            public ringkasanFasilitas ringkasanFasilitas { get; set; }
            public fasilitas fasilitas { get; set; }
        }

        public class parameterPencarianPerusahaan
        {
            public string namaBadanUsaha { get; set; }
            public string npwp { get; set; }
            public string tempatPendirian { get; set; }
            public string tanggalAktaPendirian { get; set; }
            public string nomorAktaPendirian { get; set; }
        }

        public class dataPokokDebiturPerusahaan
        {
            public string namaDebitur { get; set; }
            public string namaLengkap { get; set; }
            public string npwp { get; set; }
            public string bentukBu { get; set; }
            public string goPublic { get; set; }
            public string bentukBuKet { get; set; }
            public string tempatPendirian { get; set; }
            public string noAktaPendirian { get; set; }
            public string tglAktaPendirian { get; set; }
            public string pelapor { get; set; }
            public string pelaporKet { get; set; }
            public string tanggalDibentuk { get; set; }
            public string tanggalUpdate { get; set; }
            public string alamat { get; set; }
            public string kelurahan { get; set; }
            public string kecamatan { get; set; }
            public string kabKota { get; set; }
            public string kabKotaKet { get; set; }
            public string kodePos { get; set; }
            public string negara { get; set; }
            public string negaraKet { get; set; }
            public string noAktaTerakhir { get; set; }
            public string tglAktaTerakhir { get; set; }
            public string sektorEkonomi { get; set; }
            public string sektorEkonomiKet { get; set; }
            public string pemeringkat { get; set; }
            public string pemeringkatKet { get; set; }
            public string peringkat { get; set; }
            public string tanggalPemeringkatan { get; set; }
        }

    }
}