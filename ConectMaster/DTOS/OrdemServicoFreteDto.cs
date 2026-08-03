namespace ConectMaster.DTOS
{
    // DTOs/OrdemServicoFreteDto.cs
    public class OrdemServicoFreteDto
    {
        public string Titulo { get; set; }
        public string Email { get; set; }
        public string Setor { get; set; }
        public string Categoria { get; set; }
        public int Prioridade { get; set; }

        public ClienteDto Cliente { get; set; }
        public CargaDto Carga { get; set; }
        public RotaDto Rota { get; set; }
        public TransporteDto Transporte { get; set; }
        public FiscalDto Fiscal { get; set; }
        public ValoresDto Valores { get; set; }

        public string Descricao { get; set; }
    }

    public class ClienteDto
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Telefone { get; set; }
        public string Contato { get; set; }
    }

    public class CargaDto
    {
        public string Descricao { get; set; }
        public decimal? PesoBruto { get; set; }
        public decimal? Volume { get; set; }
        public int? QtdVolumes { get; set; }
        public decimal? ValorMercadoria { get; set; }
        public string NaturezaCarga { get; set; }
    }

    public class RotaDto
    {
        public string EnderecoColeta { get; set; }
        public string EnderecoEntrega { get; set; }
        public DateTime? DataColeta { get; set; }
        public DateTime? DataEntrega { get; set; }
    }

    public class TransporteDto
    {
        public string PlacaVeiculo { get; set; }
        public string TipoVeiculo { get; set; }
        public string MotoristaNome { get; set; }
        public string MotoristaTelefone { get; set; }
    }

    public class FiscalDto
    {
        public string NumeroNfe { get; set; }
        public string NumeroCte { get; set; }
    }

    public class ValoresDto
    {
        public decimal? ValorFrete { get; set; }
        public string FormaPagamento { get; set; }
    }
}
