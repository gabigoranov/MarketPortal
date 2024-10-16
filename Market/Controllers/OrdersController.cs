﻿using Market.Services.Firebase;
using Market.Services.Offers;
using Market.Services;
using Microsoft.AspNetCore.Mvc;
using Market.Services.Orders;
using Market.Data.Models;
using Market.Services.Authentication;
using System.Text.Json;

namespace Market.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;
        private readonly IFirebaseServive _firebaseService;
        private readonly IOrdersService _ordersService;
        private User user;



        public OrdersController(IUserService userService, IFirebaseServive firebaseService, IOrdersService ordersService, IAuthenticationService authService)
        {
            _userService = userService;
            _firebaseService = firebaseService;
            user = _userService.GetUser();
            _ordersService = ordersService;
            _authService = authService;
        }
        public IActionResult Index(List<Order>? orders)
        {
            if(orders != null && orders.Count > 0)
            {
                return View(orders);
            }
            user = _userService.Login(user.Email, user.Password).Result;
            _authService.SignInAsync(JsonSerializer.Serialize(user));
            return View(user.SoldOrders);
        }

        [HttpGet]
        public async Task<IActionResult> Approve(int id)
        {
            await _ordersService.ApproveOrderAsync(id);
            _userService.AddApprovedOrder(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Decline(int id)
        {
            await _ordersService.DeclineOrderAsync(id);
            await _userService.RemoveOrderAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Sort(List<Order> orders, string type)
        {
            orders = orders.OrderByDescending(x => x.Price).ToList();
            return RedirectToAction("Index", orders);
        }

        [HttpGet]
        public async Task<IActionResult> Deliver(int id)
        {
            await _ordersService.DeliverOrderAsync(id);
            _userService.AddDeliveredOrder(id);
            return RedirectToAction("Index");
        }
    }
}
