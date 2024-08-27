import React from "react";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
 
import Stock from './Stock'

export default function App() {
  return (
    <Router> 
      <Stock />
    </Router>
  );
}