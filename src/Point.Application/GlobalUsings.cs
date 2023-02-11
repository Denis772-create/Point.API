// Global using directives

global using System.ComponentModel.DataAnnotations;
global using FluentValidation;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.Extensions.Logging;
global using NetTopologySuite.Geometries;
global using Point.Application.Commands.CompanyAggregate;
global using Point.Application.Dto;
global using Point.Application.Interfaces;
global using Point.Application.Services;
global using Point.Domain.Entities.CardAggregate;
global using Point.Domain.Entities.CardTemplateAggregate;
global using Point.Domain.Entities.CompanyAggregate;
global using Point.Domain.Exceptions;
global using Point.Domain.SeedWork;
global using Point.Shared.Extensions;
global using Point.Shared.Queries.API;
global using Serilog.Context;