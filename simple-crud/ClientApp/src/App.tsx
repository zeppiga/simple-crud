import React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Novelties } from './components/Novelties';
import { Add } from './components/Add';
import './custom.css'

export default function App() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/add' component={Add} />
        <Route path='/novelties' component={Novelties} />
      </Layout>
    );
}