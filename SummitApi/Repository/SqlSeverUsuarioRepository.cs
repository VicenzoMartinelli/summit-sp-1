using SummitApi.Context;
using SummitApi.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SummitApi.Repository
{
  public class SqlSeverUsuarioRepository : IUsuarioRepository
  {
    private readonly ContextoSummit _context;

    public SqlSeverUsuarioRepository(ContextoSummit context)
    {
      _context = context;
    }

    public List<Usuario> ObterTodos()
    {
      return _context.Users.ToList();
    }
  }
}
