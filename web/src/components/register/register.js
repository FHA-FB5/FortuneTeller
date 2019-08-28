import React, { Component } from 'react';
import './register.css'

import TextField from '../textField/textField'
import Button from '../button/button'
import GenderRadio from '../genderRadio/genderRadio'
import MajorRadio from '../majorRadio/majorRadio'

class Register extends Component {
  
  constructor(props) {
    super(props)
    
    this.state = {
      name: "",
      surname: "",
      major: "",
      gender: "",
      age: "",
      valid: {
        name: null,
        surname: null,
        major: null,
        gender: null,
        age: null,
      }
    }
    
    this.handleChange = this.handleChange.bind(this)
    this.handleSubmit = this.handleSubmit.bind(this)
  }
  
  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value
    })
  }
  
  handleSubmit(e) {
    e.preventDefault()
    e.stopPropagation()
    
    let valid = {
      name: this.state.name ? true : false,
      surname: this.state.surname ? true : false,
      major: this.state.major ? true : false,
      gender: this.state.gender ? true : false,
      age: parseInt(this.state.age) ? true : false,
    }
    
    this.setState({
      valid: valid
    })
    
    if(valid.name && valid.surname && valid.major && valid.gender && valid.age) {
    
      fetch("/person", {
          method: 'POST', // *GET, POST, PUT, DELETE, etc.
          mode: 'cors', // no-cors, cors, *same-origin
          cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
          credentials: 'same-origin', // include, *same-origin, omit
          headers: {
              'Content-Type': 'application/json',
          },
          body: JSON.stringify(this.state),
      })
      .then(response => {
        if(response.status !== 200) {
          throw "error"
        }
        return response.json()
      })
      .then(res => this.props.onUserDate(res))
      .catch(error => alert("Unfortunately there was an error, please try again later."));
      
    }
  }
  
  render() {
    return (
      <div className="register-wrapper">
        <TextField placeholder="Max" label="Vorname | Name" name="name" onChange={this.handleChange} valid={this.state.valid.name} />
        <TextField placeholder="Mustermann" label="Nachname | Surname" name="surname" onChange={this.handleChange} valid={this.state.valid.surname} />
        <MajorRadio label="Studiengang | Major" onChange={this.handleChange} valid={this.state.valid.major} />
        <GenderRadio label="Geschlecht | Gender" onChange={this.handleChange} valid={this.state.valid.gender} />
        <TextField placeholder="20" label="Alter | Age" name="age" onChange={this.handleChange} valid={this.state.valid.age} />
        <Button value="Submit" onClick={this.handleSubmit} />
      </div>
    )
  }
  
}

export default Register