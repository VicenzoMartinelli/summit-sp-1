using System.Collections.Generic;
using SummitApi.Domain;

namespace SummitApi.Repository
{
    public class UsuarioEmMemoriaRepository : IUsuarioRepository
  {
        private readonly List<Usuario> users; 

        public UsuarioEmMemoriaRepository()
        {
            users = new List<Usuario>(){
                new Usuario(){
                    QualquerCoisa = "1"
                },
                new Usuario(){
                    QualquerCoisa = "2"
                },
                new Usuario(){
                    QualquerCoisa = "3"
                },
            };
        }
        public List<Usuario> ObterTodos()
        {
            return users;
        }
    }
}