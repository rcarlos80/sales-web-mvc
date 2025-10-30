using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        //injeção de dependência (quando SellerService for criado, receberá uma instância de SalesWebMvcContext)
        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            //return _context.Seller.FirstOrDefault(obj => obj.Id == id);
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id); //inclui o Department
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }

        public void Update(Seller obj)
        {
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found!");
            }
            try
            {
            _context.Update(obj);
            _context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException e) //intercepta a exceção da camada de acesso a dados...
            {
                throw new DbConcurrencyException(e.Message); //...e relança no nível da própria camada (Serviço), sem propagar
            }
        }

    }
}
