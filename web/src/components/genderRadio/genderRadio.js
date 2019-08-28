import React, { Component } from 'react' 
import './genderRadio.css'

class GenderRadio extends Component {
  
  render() {
    let cn = "genderRadio-wrapper"
    
    if(this.props.valid !== null) {
      if (this.props.valid) {
        cn += " valid"
      } else {
        cn += " invalid"
      }
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
