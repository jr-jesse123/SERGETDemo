using AutoMapper;
using SERGETStore.App.ViewModels;
using SERGETStore.Business.Models;

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
