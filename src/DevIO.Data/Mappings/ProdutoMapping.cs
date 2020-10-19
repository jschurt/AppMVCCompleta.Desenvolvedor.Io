using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Data.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {

            builder
                .ToTable("Produtos");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder
                .Property(p => p.Descricao)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder
                .Property(p => p.Imagem)
                .IsRequired()
                .HasColumnType("varchar(100)");

            

        } //Configure
    } //class
} //namespace
