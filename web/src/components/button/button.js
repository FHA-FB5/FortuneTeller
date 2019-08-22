import React, { Component } from 'react'
import './button.css'


class Button extends Component {
  
  render() {
    return (
      <div>
        <button onClick={this.props.onClick} className="button" >{ this.props.value }</button>
      </div>
    )
  }
  
}

export default Button