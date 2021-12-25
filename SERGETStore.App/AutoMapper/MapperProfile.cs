using AutoMapper;
using DevIO.Business.Models;
using SERGETStore.App.ViewModels;

namespace SERGETStore.App.AutoMapper
{
    public class MapperProfile : Profile
    {
        //TODO: TESTAR PROFILES
        public MapperProfile()
        {
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<Produto, ProdutoViewModel>().ReverseMap();
        }
    }
}
