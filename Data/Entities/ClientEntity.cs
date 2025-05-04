﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(ClientName), IsUnique = true)]
public class ClientEntity
{
    [Key, Column(TypeName = "varchar(36)")]
    public string Id { get; set; } = null!;

    [Column(TypeName = "nvarchar(150)")]
    public string ClientName { get; set; } = null!;

    [Column(TypeName = "nvarchar(150)")]
    public string Email { get; set; } = null!;

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
