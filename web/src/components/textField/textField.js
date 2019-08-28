import React, { Component } from 'react' 
import './textField.css'

class TextField extends Component {
  
  render() {
    let cn = "textfield-wrapper"
    
    if(this.props.valid !== null) {
      if (this.props.valid) {
        cn += " valid"
      } else {
        cn += " invalid"
      }
    }
    
    return (
      <div className={ cn }>
        <label htmlFor={ this.props.name } className="textfield-label">{ this.props.label }</label>
        <div className="textfield-inner">
          <input placeholder={ this.props.placeholder } name={ this.props.name } className="textfield-input" type={ this.props.type || "text" } value={ this.props.value } onChange={ this.props.onChange } />
        </div>
      </div>      
    )
  }
    
}

export default TextField
