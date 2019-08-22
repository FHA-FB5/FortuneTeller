import React, { Component } from 'react';
import './styles/global.css';

import Register from './components/register/register';
import GroupView from './components/groupView/groupView';
import Wave from './components/wave/wave';
import './App.css';

class App extends Component {
  constructor(props) {
    super(props)
    
    this.state = {
      groupInfo: null
    }
    
    this.handleUserData = this.handleUserData.bind(this)
  }
  
  handleUserData(data) {
    fetch("/group/"+data.groupId)
      .then(response => response.json())
      .then(res => {
        if(res.message) {
          alert("Unfortunately there was an error, please try again later.")
        } else {
          this.setState({
            groupInfo: res
          })
        }
      })
      .catch(error => alert("Unfortunately there was an error, please try again later."))
  }
  
  render() { 
    return (
      <div className="App">
        <header className="App-header">
          
        </header>
        <Wave />
        <div className="App-body">
          <div className="card">
            { !this.state.groupInfo ? (<Register onUserDate={ this.handleUserData } />) : (<GroupView groupInfo={ this.state.groupInfo } />)}
          </div> 
        </div>
        <footer className="App-footer">
          <p>UI made with 💚 by Jan-Niklas</p>
        </footer>
      </div>
    )
  }
}

export default App;
