using GeradorTestes.Dominio.ModuloMateria;

namespace GeradorTestes.Infra.Orm.ModuloMateria
{
    public class MapeadorMateriaOrm : IEntityTypeConfiguration<Materia>
    {
        public void Configure(EntityTypeBuilder<Materia> materiaBuilder)
        {
            materiaBuilder.ToTable("TBMateria");
            materiaBuilder.Property(m => m.Id).IsRequired(true).ValueGeneratedNever();
            materiaBuilder.Property(m => m.Nome).HasColumnType("varchar(100)").IsRequired();
            materiaBuilder.Property(m => m.Serie).HasConversion<int>().IsRequired();

            materiaBuilder.HasOne(m => m.Disciplina)
                .WithMany(d => d.Materias)
                .IsRequired()
                .HasConstraintName("FK_TBMateria_TBDisciplina")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
