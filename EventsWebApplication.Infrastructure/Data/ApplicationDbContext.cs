﻿using EventsWebApplication.Domain.Entities.Events;
using EventsWebApplication.Domain.Entities.Perticipants;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);


    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }
}