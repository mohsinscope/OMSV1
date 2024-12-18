using System;
using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSV1.Application.CQRS.Users.Commands;
using OMSV1.Application.Helpers;
using OMSV1.Domain.SeedWork;
using OMSV1.Infrastructure.Identity;

namespace OMSV1.Application.CQRS.Users.Handlers;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, IActionResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<AppRole> roleManager,
        IMapper mapper,
        IGenericRepository<OMSV1.Domain.Entities.Profiles.Profile> profileRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return null;

    }
}
