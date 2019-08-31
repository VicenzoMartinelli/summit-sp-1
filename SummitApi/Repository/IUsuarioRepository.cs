using SummitApi.Domain;
using System.Collections.Generic;

namespace SummitApi.Repository
{
  public interface IUsuarioRepository
  {
    List<Usuario> ObterTodos();
  }
}
