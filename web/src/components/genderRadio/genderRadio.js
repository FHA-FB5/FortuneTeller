import React, { Component } from 'react' 
import './genderRadio.css'

class GenderRadio extends Component {
  
  render() {
    let cn = "genderRadio-wrapper"
    
    switch (this.props.valid) {
      case true:
        cn += " valid"
        break;
      case false:
        cn += " invalid"
        break;
      default:
    }
    
    return (
      <div className={ cn }>
        <label className="genderRadio-label">{ this.props.label }</label>
        <div className="genderRadio-inner">
          <div>
            <input name="gender" className="genderRadio-input" type="radio" value="female" onChange={ this.props.onChange } />
            <label className="genderRadio-inner-label">weiblich</label>
          </div>
          <div>
            <input name="gender" className="genderRadio-input" type="radio" value="male" onChange={ this.props.onChange } />
            <label className="genderRadio-inner-label">männlich</label>
          </div>
        </div>
      </div>      
    )
  }
    
}

export default GenderRadio