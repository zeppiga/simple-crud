import React, { useState } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export function NavMenu() {
  const [collapsed, setCollapsed] = useState(true);

  // todo
  function toggleNavbar() {
    setCollapsed(prev => !prev);
  }

  function goToSwaggerPortal() {
    window.location.replace("/swagger");
  }

  return (
    <header>
         <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
           <Container>
             <NavbarBrand tag={Link} to="/">simple_crud</NavbarBrand>
             <NavbarToggler onClick={toggleNavbar} className="mr-2" /> 
             <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
               <ul className="navbar-nav flex-grow">
                 <NavItem>
                   <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                 </NavItem>
                 <NavItem>
                   <NavLink tag={Link} className="text-dark" to="/add">Add a novelty</NavLink>
                 </NavItem>
                 <NavItem>
                   <NavLink tag={Link} className="text-dark" to="/novelties">List of novelties</NavLink>
                 </NavItem>
                 <NavItem>
                   <NavLink tag={Link} className="text-dark" to="/" onClick={goToSwaggerPortal}>Swagger UI</NavLink>
                 </NavItem>
               </ul>
             </Collapse>
           </Container>
         </Navbar>
       </header>
  )
}